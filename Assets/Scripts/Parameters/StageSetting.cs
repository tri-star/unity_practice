using UnityEngine;

namespace ActionSample.Parameters
{
    [CreateAssetMenu(menuName = "ActionSample/Stage Setting")]
    public class StageSetting : ScriptableObject
    {
        /// <summary>
        /// 重力加速度
        /// </summary>
        public float gravitySpeed;

        /// <summary>
        /// 重力による加速の最大値
        /// </summary>
        public float maxGravitySpeed;
    }

}

