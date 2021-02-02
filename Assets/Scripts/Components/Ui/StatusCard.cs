using System;
using ActionSample.Components.Unit;
using ActionSample.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace ActionSample.Components.Ui
{
    public class StatusCard : MonoBehaviour, IDisposable
    {
        public enum STATUS
        {
            OPEN,
            CLOSING,
            CLOSED
        }

        public class StatusCardCloseEvent
        {
            public IUnit Unit { get; private set; }

            public StatusCardCloseEvent(IUnit unit)
            {
                Unit = unit;
            }
        }

        private GameObject hpGauge;

        private Animator animator;

        private IUnit unit;

        public STATUS state { get; private set; }

        public UnityEvent<StatusCardCloseEvent> CloseEvent { get; private set; }

        public void Awake()
        {
            hpGauge = transform.Find("HpGauge").gameObject;
            animator = GetComponent<Animator>();
            state = STATUS.OPEN;
            CloseEvent = new UnityEvent<StatusCardCloseEvent>();
        }

        public void FixedUpdate()
        {
            if (state == STATUS.CLOSING)
            {
                if (AnimationUtil.IsAnimationDone(animator, "StatusCardClose"))
                {
                    state = STATUS.CLOSED;
                    CloseEvent.Invoke(new StatusCardCloseEvent(unit));
                    Destroy(gameObject);
                }
            }
        }


        public void Init(IUnit unit, Vector2 position)
        {
            this.unit = unit;
            transform.localPosition = position;
            hpGauge.GetComponent<HpGauge>().Init(unit);
            animator.SetBool("isClosing", false);
        }

        public void Close()
        {
            state = STATUS.CLOSING;
            animator.SetBool("isClosing", true);
        }

        public void SetDestination(Vector2 position)
        {
            transform.localPosition = position;
        }


        public void Dispose()
        {
            CloseEvent.RemoveAllListeners();
        }
    }
}
