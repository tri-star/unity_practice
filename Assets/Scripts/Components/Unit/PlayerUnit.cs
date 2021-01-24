using System;
using System.Collections;
using ActionSample.Domain;
using ActionSample.Parameters;
using ActionSample.Signals;
using ActionSample.Utils;
using UnityEngine;
using Zenject;

namespace ActionSample.Components.Unit
{
    public class PlayerUnit : Unit, IDisposable
    {
        private Health health;

        private bool damageFlowStarted;

        [Inject]
        public new void Initialize()
        {
            base.Initialize();
            damageFlowStarted = false;
            signalBus.Subscribe<PlayerAttackSignal>(OnAttackEvent);
            health = GetComponent<Health>();
        }

        public void Dispose()
        {
            signalBus.Unsubscribe<PlayerAttackSignal>(OnAttackEvent);
        }

        public void OnAttackEvent(PlayerAttackSignal signal)
        {
            Vector3 force = stageSetting.defaultDamageForce;
            if (Dimension == Unit.DIMENSION.LEFT)
            {
                force = new Vector3(force.x * -1, force.y, force.z);
            }

            this.GetComponent<CombatUnit>().weaponPower = new WeaponPower(
                health.Power,
                force
            );
            TrySetState(Unit.STATES.ATTACK);
        }


        public new void FixedUpdate()
        {
            base.FixedUpdate();

            switch (GetState())
            {
                case Unit.STATES.WALKING:
                    if (Mathf.Abs(Velocity.x) == 0 && Mathf.Abs(Velocity.z) == 0)
                    {
                        TrySetState(Unit.STATES.NEUTRAL);
                    }
                    break;
                case Unit.STATES.NEUTRAL:
                    if (Mathf.Abs(Velocity.x) > 0 || Mathf.Abs(Velocity.z) > 0)
                    {
                        TrySetState(Unit.STATES.WALKING);
                    }
                    break;
                case Unit.STATES.ATTACK:
                    if (AnimationUtil.IsAnimationDone(animator, "PlayerAttack"))
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

        private IEnumerator DamageStateFlow()
        {
            // @TODO: ダメージ時間もパラメータ化する
            yield return new WaitForSeconds(0.5f);
            TrySetState(Unit.STATES.NEUTRAL);
            damageFlowStarted = false;
        }

    }
}
