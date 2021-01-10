using System;
using System.Collections;
using ActionSample.Signals;
using ActionSample.Utils;
using UnityEngine;
using Zenject;

namespace ActionSample.Components
{
    public class JimmyUnit : Unit, IDisposable
    {
        private bool _damageFlowStarted;

        [Inject]
        public new void Initialize()
        {
            base.Initialize();
            _signalBus.Subscribe<UnitStateChangeSignal>(OnUnitStateChange);
            _damageFlowStarted = false;
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<UnitStateChangeSignal>(OnUnitStateChange);
        }

        public void Update()
        {
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


        public void OnUnitStateChange(UnitStateChangeSignal signal)
        {
            switch (signal.newState)
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
                    _animator.SetBool("isDamaged", true);
                    break;
            }
        }

    }
}
