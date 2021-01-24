using UnityEngine;

namespace ActionSample.Domain
{
    /// <summary>
    /// 攻撃によるダメージの情報を保持するクラス
    /// </summary>
    public class WeaponPower
    {
        public float Power
        {
            get; set;
        }
        public Vector3 Force
        {
            get; set;
        }


        public WeaponPower(float power, Vector3 force)
        {
            Power = power;
            Force = force;
        }
    }

}
