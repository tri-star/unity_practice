using UnityEngine;

namespace ActionSample.Parameters.Unit
{
    [CreateAssetMenu(menuName = "ActionSample/Health Setting")]
    public class HealthSetting : ScriptableObject
    {
        public double hp;
        public double power;
    }

}
