using System;
using System.Collections.Generic;
using ActionSample.Infrastructure.RandomGenerator;
using NUnit.Framework;

namespace Tests.EditMode.Domain.RandomGeneerator
{
    public class RandomGeneratorStubTest
    {

        private RandomGeneratorStub generator;

        [SetUp]
        public void SetUp()
        {
            generator = new RandomGeneratorStub();
        }

        [Test]
        public void _値をセットした順に取得できること()
        {
            List<float> values = new List<float>() { 10, 20, 30 };

            generator.SetValues(values);
            float v = generator.FromRange(0, 0);
            Assert.That(v, Is.EqualTo(10));

            v = generator.FromRange(0, 0);
            Assert.That(v, Is.EqualTo(20));

            v = generator.FromRange(0, 0);
            Assert.That(v, Is.EqualTo(30));
        }


        [Test]
        public void _セットした以上に値を引き出そうとすると例外がスローされる()
        {
            List<float> values = new List<float>() { 10, 20 };

            generator.SetValues(values);
            float v = generator.FromRange(0, 0);
            Assert.That(v, Is.EqualTo(10));

            v = generator.FromRange(0, 0);
            Assert.That(v, Is.EqualTo(20));

            Assert.That(() =>
            {
                generator.FromRange(0, 0);
            }, Throws.TypeOf<Exception>());
        }

    }
}
