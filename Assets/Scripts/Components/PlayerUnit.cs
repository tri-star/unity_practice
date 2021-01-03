using System;
using ActionSample.Signals;
using UnityEngine;
using Zenject;

namespace ActionSample.Components
{
    public class PlayerUnit : MonoBehaviour, IInitializable, IDisposable
    {
        [Inject]
        private SignalBus _signalBus;

        private Animator _animator;

        [Inject]
        public void Initialize()
        {
            _animator = GetComponent<Animator>();
            _signalBus.Subscribe<PlayerAttackSignal>(OnAttackEvent);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<PlayerAttackSignal>(OnAttackEvent);
        }

        public void OnAttackEvent(PlayerAttackSignal signal)
        {
            Debug.Log("received attack event");
        }
    }
}
