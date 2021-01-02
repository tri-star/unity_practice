using UnityEngine;
using ActionSample.Parameters.Unit;

namespace ActionSample.Components
{
    public class Health : MonoBehaviour, IComponent
    {
        [SerializeField]
        private HealthSetting _initialHealth;

        private double _currentHp;

        private double _maxHp;
        private double _power;

        public void Start()
        {
            _currentHp = _initialHealth.hp;
            _maxHp = _initialHealth.hp;
            _power = _initialHealth.power;
        }

        public double currentHp { get { return _currentHp; } }

        public double maxHp { get { return _maxHp; } }

        public double power { get { return _power; } }
    }

}
