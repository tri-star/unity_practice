
using System.Collections.Generic;
using ActionSample.Components;
using ActionSample.Domain;
using ActionSample.Domain.BehaviourTree;
using NUnit.Framework;
using UnityEngine;
using Zenject;

namespace Tests.EditMode.Domain.BehaviourTree
{
    class BehaviourConditionStub : IBehaviourCondition
    {
        private bool isSatisfiedFlag;
        public BehaviourConditionStub(bool isSatisfied)
        {
            isSatisfiedFlag = isSatisfied;
        }
        public bool isSatisfied(GameContext context, IUnit unit)
        {
            return isSatisfiedFlag;
        }
    }

    class BaseTestCase : ZenjectUnitTestFixture
    {
        protected GameObject gameObject;

        protected GameContext gameContext;

        [SetUp]
        public void CommonInstall()
        {
            gameObject = new GameObject();
            gameObject.AddComponent<PlayerUnit>();

            GameContext context = EditModeUtil.CreateGameContext();
            Container.Bind<GameContext>().FromInstance(context).AsSingle();
        }
    }

    class BehaviourConditionAndTest : BaseTestCase
    {
        [Test]
        public void _全ての条件がTRUEを返した場合__TRUEを返す()
        {
            List<IBehaviourCondition> conditions = new List<IBehaviourCondition>
            {
                new BehaviourConditionStub(true),
                new BehaviourConditionStub(true),
                new BehaviourConditionStub(true)
            };

            var dummyUnit = gameObject.GetComponent<IUnit>();
            var behaviourCondition = BehaviourCondition.And(conditions);
            Assert.That<bool>(behaviourCondition.isSatisfied(gameContext, dummyUnit), Is.EqualTo(true));
        }

        [Test]
        public void _一部の条件がFALSEを返した場合__FALSEを返す()
        {
            List<IBehaviourCondition> conditions = new List<IBehaviourCondition>
            {
                new BehaviourConditionStub(true),
                new BehaviourConditionStub(false),
                new BehaviourConditionStub(true)
            };

            var dummyUnit = gameObject.GetComponent<IUnit>();
            var behaviourCondition = BehaviourCondition.And(conditions);
            Assert.That<bool>(behaviourCondition.isSatisfied(gameContext, dummyUnit), Is.EqualTo(false));
        }

        [Test]
        public void _条件が存在しない場合__TRUEを返す()
        {
            List<IBehaviourCondition> conditions = new List<IBehaviourCondition>
            {
            };

            var dummyUnit = gameObject.GetComponent<IUnit>();
            var behaviourCondition = BehaviourCondition.And(conditions);
            Assert.That<bool>(behaviourCondition.isSatisfied(gameContext, dummyUnit), Is.EqualTo(true));
        }
    }


    class BehaviourConditionOrTest : BaseTestCase
    {
        [Test]
        public void _全ての条件がTRUEを返した場合__TRUEを返す()
        {
            List<IBehaviourCondition> conditions = new List<IBehaviourCondition>
            {
                new BehaviourConditionStub(true),
                new BehaviourConditionStub(true),
                new BehaviourConditionStub(true)
            };

            var dummyUnit = gameObject.GetComponent<IUnit>();
            var behaviourCondition = BehaviourCondition.Or(conditions);
            Assert.That<bool>(behaviourCondition.isSatisfied(gameContext, dummyUnit), Is.EqualTo(true));
        }

        [Test]
        public void _一部の条件がFALSEを返した場合__TRUEを返す()
        {
            List<IBehaviourCondition> conditions = new List<IBehaviourCondition>
            {
                new BehaviourConditionStub(true),
                new BehaviourConditionStub(false),
                new BehaviourConditionStub(true)
            };

            var dummyUnit = gameObject.GetComponent<IUnit>();
            var behaviourCondition = BehaviourCondition.Or(conditions);
            Assert.That<bool>(behaviourCondition.isSatisfied(gameContext, dummyUnit), Is.EqualTo(true));
        }

        [Test]
        public void _全ての条件がFALSEを返した場合__FALSEを返す()
        {
            List<IBehaviourCondition> conditions = new List<IBehaviourCondition>
            {
                new BehaviourConditionStub(false),
                new BehaviourConditionStub(false),
                new BehaviourConditionStub(false)
            };

            var dummyUnit = gameObject.GetComponent<IUnit>();
            var behaviourCondition = BehaviourCondition.Or(conditions);
            Assert.That<bool>(behaviourCondition.isSatisfied(gameContext, dummyUnit), Is.EqualTo(false));
        }

        [Test]
        public void _条件が存在しない場合__TRUEを返す()
        {
            List<IBehaviourCondition> conditions = new List<IBehaviourCondition>
            {
            };

            var dummyUnit = gameObject.GetComponent<IUnit>();
            var behaviourCondition = BehaviourCondition.Or(conditions);
            Assert.That<bool>(behaviourCondition.isSatisfied(gameContext, dummyUnit), Is.EqualTo(true));
        }
    }


    class BehaviourConditionNotTest : BaseTestCase
    {
        [Test]
        public void _渡した条件がTRUEを返した場合__FALSEを返す()
        {
            IBehaviourCondition inputCondition = new BehaviourConditionStub(true);

            var dummyUnit = gameObject.GetComponent<IUnit>();
            var behaviourCondition = BehaviourCondition.Not(inputCondition);
            Assert.That<bool>(behaviourCondition.isSatisfied(gameContext, dummyUnit), Is.EqualTo(false));
        }

        [Test]
        public void _渡した条件がFALSEを返した場合__TRUEを返す()
        {
            IBehaviourCondition inputCondition = new BehaviourConditionStub(false);

            var dummyUnit = gameObject.GetComponent<IUnit>();
            var behaviourCondition = BehaviourCondition.Not(inputCondition);
            Assert.That<bool>(behaviourCondition.isSatisfied(gameContext, dummyUnit), Is.EqualTo(true));
        }
    }

}
