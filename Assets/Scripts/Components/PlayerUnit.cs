using System;
using ActionSample.Domain;
using ActionSample.Parameters;
using ActionSample.Signals;
using ActionSample.Utils;
using UnityEngine;
using Zenject;

namespace ActionSample.Components
{
    public class PlayerUnit : Unit, IDisposable
    {
        private Health _health;

        [Inject]
        public new void Initialize()
        {
            base.Initialize();
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

            this.GetComponent<Weapon>().damage = new Damage(
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

        protected new bool CanTransitionState(Unit.States newState)
        {
            // @TODO: ドメインに関する実装として切り出す
            switch (GetState())
            {
                case Unit.States.NEUTRAL:
                    return true;
                case Unit.States.WALKING:
                    return true;
                case Unit.States.ATTACK:
                    if (newState == Unit.States.WALKING)
                    {
                        return false;
                    }
                    return true;
                case Unit.States.DAMAGE:
                    if (newState != Unit.States.NEUTRAL)
                    {
                        return false;
                    }
                    return true;
            }
            throw new NotSupportedException($"無効な状態が指定されました: {GetState()}");
        }


        protected new void UpdateAnimator()
        {
            switch (GetState())
            {
                case Unit.States.WALKING:
                    _animator.SetBool("isAttacking", false);
                    _animator.SetBool("isWalking", true);
                    break;
                case Unit.States.ATTACK:
                    _animator.SetBool("isAttacking", true);
                    _animator.SetBool("isWalking", false);
                    break;
                default:
                    _animator.SetBool("isAttacking", false);
                    _animator.SetBool("isWalking", false);
                    break;
            }
        }
    }
}
