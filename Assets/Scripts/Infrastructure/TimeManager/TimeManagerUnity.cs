using ActionSample.Domain;
using UnityEngine;

namespace ActionSample.Infrastructure.TimeManager
{
    public class TimeManagerUnity : ITimeManager
    {
        public float GetDeltaTime()
        {
            return Time.deltaTime;
        }
    }
}
