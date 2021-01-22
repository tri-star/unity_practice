using System;
using System.Collections;
using ActionSample.Domain;
using ActionSample.Domain.Behaviour;
using ActionSample.Domain.BehaviourTree;
using ActionSample.Signals;
using ActionSample.Utils;
using UnityEngine;
using Zenject;

namespace ActionSample.Components
{
    public class JimmyUnit : Unit, IInitializable, IDisposable
    {
        private bool _damageFlowStarted;

        private bool _deadFlowStarted;

        [Inject]
        private GameContext gameContext;

        private BehaviourExecutor behaviourExecutor;

        public bool enableAI = true;

        [Inject]
        public new void Initialize()
        {
            base.Initialize();
            this._animator = this.GetComponent<Animator>();
            _damageFlowStarted = false;
            _deadFlowStarted = false;
            IBehaviourTreeBuilder builder = new JimmyBehaviour();
            behaviourExecutor = new BehaviourExecutor(builder.Build());
        }

        public void Dispose()
        {
        }

        public void Update()
        {
            if (enableAI)
            {
                var plan = behaviourExecutor.Execute(gameContext, GetComponent<IUnit>());
                plan?.Execute(gameContext, GetComponent<IUnit>());
            }

            switch (GetState())
            {
                case Unit.States.WALKING:
                    if (Mathf.Abs(velocity.x) == 0 && Mathf.Abs(velocity.z) == 0)
                    {
                        TrySetState(Unit.States.NEUTRAL);
                    }
                    break;
                case Unit.States.NEUTRAL:
                    if (Mathf.Abs(velocity.x) > 0 || Mathf.Abs(velocity.z) > 0)
                    {
                        TrySetState(Unit.States.WALKING);
                    }
                    break;
                case Unit.States.DAMAGE:
                    if (!_damageFlowStarted)
                    {
                        _damageFlowStarted = true;
                        StartCoroutine(DamageStateFlow());
                    }
                    break;
                case Unit.States.DEAD:
                    if (!_deadFlowStarted)
                    {
                        _deadFlowStarted = true;
                        StartCoroutine(DeadStateFlow());
                    }
                    break;
            }

            if (dimension == Unit.Dimension.LEFT)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            UpdateAnimator();
        }


        protected new void OnChangeState(States newState)
        {
            switch (newState)
            {
                case Unit.States.DAMAGE:
                    _damageFlowStarted = false;
                    break;
            }
        }

        private IEnumerator DamageStateFlow()
        {
            // @TODO: ダメージ時間もパラメータ化する
            yield return new WaitForSeconds(1.0f);
            TrySetState(Unit.States.NEUTRAL);
            _damageFlowStarted = false;
        }


        private IEnumerator DeadStateFlow()
        {
            affectGravity = false;
            yield return new WaitForSeconds(3.0f);
            Destroy(gameObject);
        }


        protected new void UpdateAnimator()
        {
            _animator.SetBool("isAttacking", false);
            _animator.SetBool("isWalking", false);
            _animator.SetBool("isDamaged", false);
            switch (GetState())
            {
                case Unit.States.WALKING:
                    _animator.SetBool("isWalking", true);
                    break;
                case Unit.States.ATTACK:
                    _animator.SetBool("isAttacking", true);
                    break;
                case Unit.States.DAMAGE:
                case Unit.States.DEAD:
                    _animator.SetBool("isDamaged", true);
                    break;
            }
        }

    }
}
