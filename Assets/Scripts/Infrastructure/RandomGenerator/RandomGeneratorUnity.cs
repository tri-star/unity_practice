using ActionSample.Domain.RandomGenerator;
using UnityEngine;

namespace ActionSample.Infrastructure.RandomGenerator
{
    public class RandomGeneratorUnity : IRandomGenerator
    {
        private int seed;

        private Random.State state;

        public RandomGeneratorUnity(int seed)
        {
            Init(seed);
        }


        public void Init(int seed)
        {
            this.seed = seed;
            Random.InitState(seed);
            state = Random.state;
        }


        public float FromRange(float min, float max)
        {
            Random.state = state;
            var value = Random.Range(min, max);
            state = Random.state;
            return value;
        }

    }
}
