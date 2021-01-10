using ActionSample.Domain;
using UnityEngine;

namespace ActionSample.Components
{
    public class Weapon : MonoBehaviour, IComponent
    {
        /// <summary>
        /// 攻撃によるダメージの情報
        /// </summary>
        public Damage damage
        {
            get; set;
        }
    }

}

