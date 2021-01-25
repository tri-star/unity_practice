using ActionSample.Components.Unit;
using UnityEngine;
using static ActionSample.Components.Unit.Health;

namespace ActionSample.Components.Ui
{
    public class HpGauge : MonoBehaviour
    {
        private GameObject gaugeComponent;

        private float gaugeWidth;

        private float realWidth;

        private float currentWidth;

        private IUnit unit;

        private Health health;

        void Start()
        {
            gaugeComponent = transform.Find("Current").gameObject;
            gaugeWidth = gaugeComponent.GetComponent<RectTransform>().sizeDelta.x;
            realWidth = gaugeWidth;
            currentWidth = gaugeWidth;
            //Start()よりも先に処理が走るケースがある
            //unit = null;
            //health = null;
        }

        public void Init(IUnit unit)
        {
            this.unit = unit;
            health = unit.GetComponent<Health>();
            health.DamageEvent.AddListener(OnDamage);
        }

        void FixedUpdate()
        {
            if (health == null)
            {
                return;
            }

            // TODO: アニメーションさせる
            currentWidth = realWidth;

            float h = gaugeComponent.GetComponent<RectTransform>().sizeDelta.y;
            gaugeComponent.GetComponent<RectTransform>().sizeDelta = new Vector2(currentWidth, h);
        }


        public void OnDamage(UnitDamageEvent e)
        {
            realWidth = CalcRealGaugeWidth();
        }


        public bool IsDisposed()
        {
            return false;
        }

        void OnDestroy()
        {
            gaugeComponent = null;
            unit = null;
            health = null;
        }


        private float CalcRealGaugeWidth()
        {
            var rate = Mathf.Clamp(health.CurrentHp / health.MaxHp, 0, 1);
            return gaugeWidth * rate;
        }
    }
}
