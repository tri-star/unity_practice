using System;
using ActionSample.Domain;
using ActionSample.Parameters;
using ActionSample.Signals;
using ActionSample.Utils;
using UnityEngine;
using Zenject;

namespace ActionSample.Components
{
    public class PlayerUnit : MonoBehaviour, IInitializable, IDisposable
    {
        [Inject]
        private SignalBus _signalBus;

        [Inject]
        StageSetting _stageSetting;

        private Animator _animator;

        private Unit _unit;

        private Health _health;

        [Inject]
        public void Initialize()
        {
            _animator = GetComponent<Animator>();
            _signalBus.Subscribe<PlayerAttackSignal>(OnAttackEvent);
            _unit = GetComponent<Unit>();
            _health = GetComponent<Health>();
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<PlayerAttackSignal>(OnAttackEvent);
        }

        public void OnAttackEvent(PlayerAttackSignal signal)
        {
            Vector3 force = _stageSetting.defaultDamageForce;
            if (_unit.dimension == Unit.Dimension.LEFT)
            {
                force = new Vector3(force.x * -1, force.y, force.z);
            }

            this.GetComponent<Weapon>().damage = new Damage(
                _health.power,
                force
            );
            TrySetState(Unit.States.ATTACK);
        }


        public void Update()
        {
            switch (_unit.GetState())
            {
                case Unit.States.WALKING:
                    if (Mathf.Abs(_unit.velocity.x) == 0 && Mathf.Abs(_unit.velocity.z) == 0)
                    {
                        TrySetState(Unit.States.NEUTRAL);
                    }
                    break;
                case Unit.States.NEUTRAL:
                    if (Mathf.Abs(_unit.velocity.x) > 0 || Mathf.Abs(_unit.velocity.z) > 0)
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

            if (_unit.dimension == Unit.Dimension.LEFT)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            UpdateAnimator();
        }

        private bool TrySetState(Unit.States newState)
        {
            if (CanTransitionState(newState))
            {
                _unit.SetState(newState);
                return true;
            }
            return false;
        }

        private bool CanTransitionState(Unit.States newState)
        {
            // @TODO: ドメインに関する実装として切り出す
            switch (_unit.GetState())
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
            throw new NotSupportedException($"無効な状態が指定されました: {_unit.GetState()}");
        }


        private void UpdateAnimator()
        {
            switch (_unit.GetState())
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
