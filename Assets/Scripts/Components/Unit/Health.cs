using UnityEngine;
using ActionSample.Parameters.Unit;

namespace ActionSample.Components.Unit
{
    public class Health : MonoBehaviour, IComponent
    {
        [SerializeField]
        private HealthSetting initialHealth;

        private IUnit unit;

        public void Start()
        {
            CurrentHp = initialHealth.hp;
            MaxHp = initialHealth.hp;
            Power = initialHealth.power;
            unit = GetComponent<IUnit>();
        }

        public float CurrentHp { get; private set; }

        public float MaxHp { get; private set; }

        public float Power { get; private set; }

        public void TakeDamage(float damage)
        {
            CurrentHp -= Mathf.Max(damage, 0);

            if (CurrentHp <= 0)
            {
                unit.SetState(Unit.STATES.DEAD);
            }
        }
    }
}
