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
        private Health _health;

        private bool _damageFlowStarted;

        [Inject]
        public new void Initialize()
        {
            base.Initialize();
            _damageFlowStarted = false;
            _signalBus.Subscribe<PlayerAttackSignal>(OnAttackEvent);
            _health = GetComponent<Health>();
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<PlayerAttackSignal>(OnAttackEvent);
        }

        public void OnAttackEvent(PlayerAttackSignal signal)
        {
            Vector3 force = _stageSetting.defaultDamageForce;
            if (dimension == Unit.Dimension.LEFT)
            {
                force = new Vector3(force.x * -1, force.y, force.z);
            }

            this.GetComponent<CombatUnit>().weaponPower = new WeaponPower(
                _health.power,
                force
            );
            TrySetState(Unit.States.ATTACK);
        }


        public new void FixedUpdate()
        {
            base.FixedUpdate();

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
                case Unit.States.ATTACK:
                    if (AnimationUtil.IsAnimationDone(_animator, "PlayerAttack"))
                    {
                        TrySetState(Unit.States.NEUTRAL);
                    }
                    break;
                case Unit.States.DAMAGE:
                    if (!_damageFlowStarted)
                    {
                        _damageFlowStarted = true;
                        StartCoroutine(DamageStateFlow());
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

        private IEnumerator DamageStateFlow()
        {
            // @TODO: ダメージ時間もパラメータ化する
            yield return new WaitForSeconds(0.5f);
            TrySetState(Unit.States.NEUTRAL);
            _damageFlowStarted = false;
        }

    }
}
