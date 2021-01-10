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

        public void Damage(Damage damage)
        {
            if (_unit.TrySetState(Unit.States.DAMAGE))
            {
                _unit.AddForce(damage.force);
            }
        }

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.tag == "attack")
            {
                Unit attacker = collision.collider.gameObject.GetComponentInParent<Unit>();
                Damage damage = collision.collider.gameObject.GetComponentInParent<Weapon>().damage;

                if (_unit.TrySetState(Unit.States.DAMAGE))
                {
                    _unit.AddForce(damage.force);
                }
            }
        }
    }
}
