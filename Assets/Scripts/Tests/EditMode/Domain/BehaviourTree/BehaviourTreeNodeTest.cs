
using System.Collections.Generic;
using ActionSample.Components;
using ActionSample.Domain.BehaviourTree;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditMode.Domain.BehaviourTree
{
    class BehaviourConditionStub : IBehaviourCondition
    {
        private bool _isSatisfied;
        public BehaviourConditionStub(bool isSatisfied)
        {
            _isSatisfied = isSatisfied;
        }
        public bool isSatisfied(IUnit unit)
        {
            return _isSatisfied;
        }
    }

    class BehaviourConditionAndTest
    {

        private GameObject _gameObject;

        [SetUp]
        public void SetUp()
        {
            _gameObject = new GameObject();
            _gameObject.AddComponent<PlayerUnit>();
        }

        [Test]
        public void _全ての条件がTRUEを返した場合__TRUEを返す()
        {
            List<IBehaviourCondition> conditions = new List<IBehaviourCondition>
            {
                new BehaviourConditionStub(true),
                new BehaviourConditionStub(true),
                new BehaviourConditionStub(true)
            };

            var dummyUnit = _gameObject.GetComponent<IUnit>();
            var behaviourCondition = BehaviourCondition.And(conditions);
            Assert.That<bool>(behaviourCondition.isSatisfied(dummyUnit), Is.EqualTo(true));
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

            var dummyUnit = _gameObject.GetComponent<IUnit>();
            var behaviourCondition = BehaviourCondition.And(conditions);
            Assert.That<bool>(behaviourCondition.isSatisfied(dummyUnit), Is.EqualTo(false));
        }

        [Test]
        public void _条件が存在しない場合__TRUEを返す()
        {
            List<IBehaviourCondition> conditions = new List<IBehaviourCondition>
            {
            };

            var dummyUnit = _gameObject.GetComponent<IUnit>();
            var behaviourCondition = BehaviourCondition.And(conditions);
            Assert.That<bool>(behaviourCondition.isSatisfied(dummyUnit), Is.EqualTo(true));
        }
    }


    class BehaviourConditionOrTest
    {
        private GameObject _gameObject;

        [SetUp]
        public void SetUp()
        {
            _gameObject = new GameObject();
            _gameObject.AddComponent<PlayerUnit>();
        }

        [Test]
        public void _全ての条件がTRUEを返した場合__TRUEを返す()
        {
            List<IBehaviourCondition> conditions = new List<IBehaviourCondition>
            {
                new BehaviourConditionStub(true),
                new BehaviourConditionStub(true),
                new BehaviourConditionStub(true)
            };

            var dummyUnit = _gameObject.GetComponent<IUnit>();
            var behaviourCondition = BehaviourCondition.Or(conditions);
            Assert.That<bool>(behaviourCondition.isSatisfied(dummyUnit), Is.EqualTo(true));
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

            var dummyUnit = _gameObject.GetComponent<IUnit>();
            var behaviourCondition = BehaviourCondition.Or(conditions);
            Assert.That<bool>(behaviourCondition.isSatisfied(dummyUnit), Is.EqualTo(true));
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

            var dummyUnit = _gameObject.GetComponent<IUnit>();
            var behaviourCondition = BehaviourCondition.Or(conditions);
            Assert.That<bool>(behaviourCondition.isSatisfied(dummyUnit), Is.EqualTo(false));
        }

        [Test]
        public void _条件が存在しない場合__TRUEを返す()
        {
            List<IBehaviourCondition> conditions = new List<IBehaviourCondition>
            {
            };

            var dummyUnit = _gameObject.GetComponent<IUnit>();
            var behaviourCondition = BehaviourCondition.Or(conditions);
            Assert.That<bool>(behaviourCondition.isSatisfied(dummyUnit), Is.EqualTo(true));
        }
    }


    class BehaviourConditionNotTest
    {
        private GameObject _gameObject;

        [SetUp]
        public void SetUp()
        {
            _gameObject = new GameObject();
            _gameObject.AddComponent<PlayerUnit>();
        }

        [Test]
        public void _渡した条件がTRUEを返した場合__FALSEを返す()
        {
            IBehaviourCondition inputCondition = new BehaviourConditionStub(true);

            var dummyUnit = _gameObject.GetComponent<IUnit>();
            var behaviourCondition = BehaviourCondition.Not(inputCondition);
            Assert.That<bool>(behaviourCondition.isSatisfied(dummyUnit), Is.EqualTo(false));
        }

        [Test]
        public void _渡した条件がFALSEを返した場合__TRUEを返す()
        {
            IBehaviourCondition inputCondition = new BehaviourConditionStub(false);

            var dummyUnit = _gameObject.GetComponent<IUnit>();
            var behaviourCondition = BehaviourCondition.Not(inputCondition);
            Assert.That<bool>(behaviourCondition.isSatisfied(dummyUnit), Is.EqualTo(true));
        }
    }

}
