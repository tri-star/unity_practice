using System;
using System.Collections.Generic;

namespace ActionSample.Domain.RandomGenerator
{
    public class RandomGeneratorManager
    {
        private Dictionary<string, IRandomGenerator> generators;

        public RandomGeneratorManager()
        {
            generators = new Dictionary<string, IRandomGenerator>();
        }

        public void Add(string name, IRandomGenerator generator)
        {
            generators.Add(name, generator);
        }

        public IRandomGenerator Get(string name)
        {
            IRandomGenerator generator;
            if (generators.TryGetValue(name, out generator))
            {
                return generator;
            }
            throw new Exception($"指定されたジェネレータが見つかりません: {name}");
        }

        public float FromRange(string name, float min, float max)
        {
            IRandomGenerator generator = Get(name);
            return generator.FromRange(min, max);
        }
    }
}
