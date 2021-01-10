using UnityEngine;

namespace ActionSample.Domain
{
    /// <summary>
    /// 攻撃によるダメージの情報を保持するクラス
    /// </summary>
    public class Damage
    {
        public float power
        {
            get; set;
        }
        public Vector3 force
        {
            get; set;
        }


        public Damage(float power, Vector3 force)
        {
            this.power = power;
            this.force = force;
        }
    }

}
