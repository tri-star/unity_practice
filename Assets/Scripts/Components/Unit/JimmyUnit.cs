using System;
using System.Collections;
using ActionSample.Domain;
using ActionSample.Domain.Behaviour;
using ActionSample.Domain.BehaviourTree;
using ActionSample.Utils;
using UnityEngine;
using Zenject;

namespace ActionSample.Components.Unit
{
    public class JimmyUnit : Unit, IInitializable, IDisposable
    {
        private bool damageFlowStarted;

        private bool deadFlowStarted;

        [Inject]
        private GameContext gameContext;

        private BehaviourExecutor behaviourExecutor;

        public bool enableAI = true;

        [Inject]
        public new void Initialize()
        {
            base.Initialize();
            this.animator = this.GetComponent<Animator>();
            damageFlowStarted = false;
            deadFlowStarted = false;
            IBehaviourTreeBuilder builder = new JimmyBehaviour();
            behaviourExecutor = new BehaviourExecutor(builder.Build());

            this.GetComponent<CombatUnit>().weaponPower = new WeaponPower(
                100,
                new Vector3(2, 10, 0)
            );
        }

        public void Dispose()
        {
        }

        public new void FixedUpdate()
        {
            base.FixedUpdate();
            switch (GetState())
            {
                case Unit.STATES.WALKING:
                    if (enableAI)
                    {
                        var plan = behaviourExecutor.Execute(gameContext, GetComponent<IUnit>());
                        plan?.Execute(gameContext, GetComponent<IUnit>());
                    }
                    if (Mathf.Abs(Velocity.x) == 0 && Mathf.Abs(Velocity.z) == 0)
                    {
                        TrySetState(Unit.STATES.NEUTRAL);
                    }
                    break;
                case Unit.STATES.NEUTRAL:
                    if (enableAI)
                    {
                        var plan = behaviourExecutor.Execute(gameContext, GetComponent<IUnit>());
                        plan?.Execute(gameContext, GetComponent<IUnit>());
                    }
                    if (Mathf.Abs(Velocity.x) > 0 || Mathf.Abs(Velocity.z) > 0)
                    {
                        TrySetState(Unit.STATES.WALKING);
                    }
                    break;
                case Unit.STATES.ATTACK:
                    if (AnimationUtil.IsAnimationDone(animator, "Enemy02Attack"))
                    {
                        TrySetState(Unit.STATES.NEUTRAL);
                    }
                    break;
                case Unit.STATES.DAMAGE:
                    if (!damageFlowStarted)
                    {
                        damageFlowStarted = true;
                        StartCoroutine(DamageStateFlow());
                    }
                    break;
                case Unit.STATES.DEAD:
                    if (!deadFlowStarted)
                    {
                        deadFlowStarted = true;
                        StartCoroutine(DeadStateFlow());
                    }
                    break;
            }

            if (Dimension == Unit.DIMENSION.LEFT)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            UpdateAnimator();
        }


        protected new void OnChangeState(STATES newState)
        {
            switch (newState)
            {
                case Unit.STATES.DAMAGE:
                    damageFlowStarted = false;
                    break;
            }
        }

        private IEnumerator DamageStateFlow()
        {
            // @TODO: ダメージ時間もパラメータ化する
            yield return new WaitForSeconds(1.0f);
            TrySetState(Unit.STATES.NEUTRAL);
            damageFlowStarted = false;
        }


        private IEnumerator DeadStateFlow()
        {
            affectGravity = false;
            yield return new WaitForSeconds(3.0f);
            Destroy(gameObject);
        }


        protected new void UpdateAnimator()
        {
            animator.SetBool("isAttacking", false);
            animator.SetBool("isWalking", false);
            animator.SetBool("isDamaged", false);
            switch (GetState())
            {
                case Unit.STATES.WALKING:
                    animator.SetBool("isWalking", true);
                    break;
                case Unit.STATES.ATTACK:
                    animator.SetBool("isAttacking", true);
                    break;
                case Unit.STATES.DAMAGE:
                case Unit.STATES.DEAD:
                    animator.SetBool("isDamaged", true);
                    break;
            }
        }

    }
}
