using ActionSample.Signals;
using UnityEngine;
using Zenject;

namespace ActionSample.Components.Ui
{
    public class ActionButton : MonoBehaviour
    {
        [Inject]
        private SignalBus _signalBus;

        private Animator _animator;


        public void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public void OnMouseDown()
        {
            _animator.SetBool("isPressed", true);
            OnButtonPressed();
        }

        public void OnMouseUp()
        {
            _animator.SetBool("isPressed", false);
        }


        private void OnButtonPressed()
        {
            _signalBus.Fire<PlayerAttackSignal>(new PlayerAttackSignal());
        }
    }
}
