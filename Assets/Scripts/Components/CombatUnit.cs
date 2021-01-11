using ActionSample.Domain;
using UnityEngine;
using Zenject;
using Collision = UnityEngine.Collision;

namespace ActionSample.Components
{

    public class CombatUnit : MonoBehaviour, IComponent
    {
        private IUnit _unit;

        [Inject]
        public void Initialize()
        {
            _unit = GetComponent<IUnit>();
        }

        public void Damage(WeaponPower weaponPower)
        {
            if (_unit.TrySetState(Unit.States.DAMAGE))
            {
                _unit.AddForce(weaponPower.force);
            }
        }

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.tag == "attack")
            {
                Unit attacker = collision.collider.gameObject.GetComponentInParent<Unit>();
                WeaponPower weaponPower = collision.collider.gameObject.GetComponentInParent<Weapon>().weaponPower;

                if (_unit.TrySetState(Unit.States.DAMAGE))
                {
                    _unit.AddForce(weaponPower.force);
                }
            }
        }
    }
}
