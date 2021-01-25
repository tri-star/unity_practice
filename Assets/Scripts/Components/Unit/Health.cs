using UnityEngine;
using ActionSample.Parameters.Unit;
using UnityEngine.Events;

namespace ActionSample.Components.Unit
{
    public class Health : MonoBehaviour, IComponent
    {
        [SerializeField]
        private HealthSetting initialHealth;

        private IUnit unit;

        public UnityEvent<UnitDamageEvent> DamageEvent { get; private set; }

        public void Start()
        {
            CurrentHp = initialHealth.hp;
            MaxHp = initialHealth.hp;
            Power = initialHealth.power;
            unit = GetComponent<IUnit>();
            DamageEvent = new UnityEvent<UnitDamageEvent>();
        }

        public float CurrentHp { get; private set; }

        public float MaxHp { get; private set; }

        public float Power { get; private set; }

        public void TakeDamage(float damage)
        {
            CurrentHp -= Mathf.Max(damage, 0);
            DamageEvent.Invoke(new UnitDamageEvent(damage, CurrentHp));

            if (CurrentHp <= 0)
            {
                unit.SetState(Unit.STATES.DEAD);
            }
        }



        public void OnDestroy()
        {
            unit = null;
            DamageEvent.RemoveAllListeners();
        }



        public class UnitDamageEvent
        {
            public float Damage { get; private set; }

            public float RemainHp { get; private set; }

            public UnitDamageEvent(float damage, float remainHp)
            {
                Damage = damage;
                RemainHp = remainHp;
            }
        }
    }
}
