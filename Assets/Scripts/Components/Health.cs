using UnityEngine;
using ActionSample.Parameters.Unit;

namespace ActionSample.Components
{
    public class Health : MonoBehaviour, IComponent
    {
        [SerializeField]
        private HealthSetting _initialHealth;

        private float _currentHp;

        private float _maxHp;
        private float _power;

        public void Start()
        {
            _currentHp = _initialHealth.hp;
            _maxHp = _initialHealth.hp;
            _power = _initialHealth.power;
        }

        public float currentHp { get { return _currentHp; } }

        public float maxHp { get { return _maxHp; } }

        public float power { get { return _power; } }
    }

}
