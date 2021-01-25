using ActionSample.Components.Unit;
using UnityEngine;

namespace ActionSample.Components.Ui
{
    public class StatusCard : MonoBehaviour
    {
        private bool isDisposed = false;

        private GameObject hpGauge;

        public void Awake()
        {
            hpGauge = transform.Find("HpGauge").gameObject;
        }

        public void FixedUpdate()
        {
        }


        public void Init(IUnit unit, Vector2 position)
        {
            transform.localPosition = position;
            hpGauge.GetComponent<HpGauge>().Init(unit);
        }
    }
}
