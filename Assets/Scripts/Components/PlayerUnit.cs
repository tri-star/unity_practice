using System;
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

        private Animator _animator;

        private Unit _unit;

        [Inject]
        public void Initialize()
        {
            _animator = GetComponent<Animator>();
            _signalBus.Subscribe<PlayerAttackSignal>(OnAttackEvent);
            _unit = GetComponent<Unit>();
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<PlayerAttackSignal>(OnAttackEvent);
        }

        public void OnAttackEvent(PlayerAttackSignal signal)
        {
            _unit.state = Unit.States.ATTACK;
        }


        public void Update()
        {
            switch (_unit.state)
            {
                case Unit.States.WALKING:
                    if (Mathf.Abs(_unit.velocity.x) == 0 && Mathf.Abs(_unit.velocity.z) == 0)
                    {
                        _unit.state = Unit.States.NEUTRAL;
                    }
                    break;
                case Unit.States.NEUTRAL:
                    if (Mathf.Abs(_unit.velocity.x) > 0 || Mathf.Abs(_unit.velocity.z) > 0)
                    {
                        _unit.state = Unit.States.WALKING;
                    }
                    break;
                case Unit.States.ATTACK:
                    if (AnimationUtil.IsAnimationDone(_animator, "PlayerAttack"))
                    {
                        _unit.state = Unit.States.NEUTRAL;
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


        private void UpdateAnimator()
        {
            switch (_unit.state)
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
