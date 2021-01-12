using System;
using System.Collections.Generic;
using ActionSample.Domain.RandomGenerator;
using ActionSample.Infrastructure.RandomGenerator;
using NUnit.Framework;

namespace Tests.EditMode.Domain.RandomGeneerator
{
    public class RandomGeneratorUnityTest
    {

        private RandomGeneratorManager manager;

        [SetUp]
        public void SetUp()
        {
            manager = new RandomGeneratorManager();
        }

        [Test]
        public void _同じSeedを指定したインスタンスからは同じ値が取得出来る()
        {
            int seed = 100;

            manager.Add("A", new RandomGeneratorUnity(seed));
            manager.Add("B", new RandomGeneratorUnity(seed));

            float valueA = manager.FromRange("A", 10, 20);
            float valueB = manager.FromRange("B", 10, 20);

            Assert.That(valueA, Is.EqualTo(valueB));
        }

    }
}
