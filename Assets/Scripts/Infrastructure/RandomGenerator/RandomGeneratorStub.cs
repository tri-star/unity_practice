using System.Collections.Generic;
using ActionSample.Domain.RandomGenerator;

namespace ActionSample.Infrastructure.RandomGenerator
{
    public class RandomGeneratorStub : IRandomGenerator
    {
        private Queue<float> values;

        public RandomGeneratorStub()
        {
            values = new Queue<float>();
        }

        public float FromRange(float min, float max)
        {
            if (values.Count == 0)
            {
                throw new System.Exception("キューに値が準備されていません");
            }
            return values.Dequeue();
        }

        public void SetValues(List<float> newValues)
        {
            values = new Queue<float>();
            foreach (float v in newValues)
            {
                values.Enqueue(v);
            }
        }

        public void AddValue(float v)
        {
            values.Enqueue(v);
        }
    }
}
