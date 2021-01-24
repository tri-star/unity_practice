using ActionSample.Domain;
using UnityEngine;
using Zenject;
using Collision = UnityEngine.Collision;

namespace ActionSample.Components.Unit
{

    public class CombatUnit : MonoBehaviour, IComponent
    {
        private IUnit unit;

        /// <summary>
        /// 攻撃によるダメージの情報
        /// </summary>
        public WeaponPower weaponPower
        {
            get; set;
        }

        [Inject]
        public void Initialize()
        {
            unit = GetComponent<IUnit>();
        }

        public void Attack()
        {
            unit.TrySetState(Unit.STATES.ATTACK);
        }

        public void Damage(WeaponPower weaponPower)
        {
            if (unit.TrySetState(Unit.STATES.DAMAGE))
            {
                unit.AddForce(weaponPower.Force);
            }
        }

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.tag == "attack")
            {
                Unit attacker = collision.collider.gameObject.GetComponentInParent<Unit>();
                WeaponPower weaponPower = collision.collider.gameObject.GetComponentInParent<CombatUnit>().weaponPower;
                Health health = GetComponent<Health>();

                if (unit.TrySetState(Unit.STATES.DAMAGE))
                {
                    unit.AddForce(weaponPower.Force);

                    // @TODO: ダメージ計算用のサービスを用意してダメージ計算を行う
                    health.TakeDamage(weaponPower.Power);
                }
            }
        }
    }
}
