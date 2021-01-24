using ActionSample.Signals;
using UnityEngine;
using Zenject;

namespace ActionSample.Components.Ui
{
    public class ActionButton : MonoBehaviour
    {
        [Inject]
        private SignalBus signalBus;

        private Animator animator;


        public void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void OnMouseDown()
        {
            animator.SetBool("isPressed", true);
            OnButtonPressed();
        }

        public void OnMouseUp()
        {
            animator.SetBool("isPressed", false);
        }


        private void OnButtonPressed()
        {
            signalBus.Fire<PlayerAttackSignal>(new PlayerAttackSignal());
        }
    }
}
