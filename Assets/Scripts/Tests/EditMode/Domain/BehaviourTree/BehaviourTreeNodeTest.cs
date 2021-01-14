#nullable enable

using System.Collections.Generic;
using ActionSample.Components;
using ActionSample.Domain;
using ActionSample.Domain.BehaviourTree;
using ActionSample.Domain.RandomGenerator;
using ActionSample.Infrastructure.RandomGenerator;
using NUnit.Framework;
using UnityEngine;
using Zenject;

namespace Tests.EditMode.Domain.BehaviourTree
{

    class TestNode : BehaviourTreeNode
    {
        public TestNode(string name, int weight = 10, List<BehaviourTreeNode>? children = null, IBehaviourCondition? condition = null, IBehaviourPlan? plan = null) : base(name, weight, children, condition, plan)
        {
        }
    }

    class BehaviourTreeNodeTest : ZenjectUnitTestFixture
    {
        [Inject]
        protected GameContext? gameContext;

        protected GameObject? gameObject;

        protected RandomGeneratorStub? randomGenerator;

        [SetUp]
        public void CommonInstall()
        {
            randomGenerator = new RandomGeneratorStub();
            var randomGeneratorMap = new Dictionary<string, IRandomGenerator>()
            {
                {"Game", randomGenerator}
            };
            var context = EditModeUtil.CreateGameContext(randomGeneratorMap: randomGeneratorMap);

            gameObject = new GameObject();
            gameObject.AddComponent<PlayerUnit>();

            Container.Bind<GameContext>().FromInstance(context).AsSingle();
            Container.Inject(this);
        }


        [Test, TestCaseSource("for_weightに応じたNodeが選択されること")]
        public void _weightに応じたNodeが選択されること(float randomValue, string expectedName)
        {
            var node = new RootBehaviourTreeNode(new List<BehaviourTreeNode>()
            {
                new TestNode(
                    name: "歩く",
                    weight: 30,
                    condition: null,
                    plan: null
                ),
                new TestNode(
                    name: "周りを見る",
                    weight: 10,
                    condition: null,
                    plan: null
                ),
            });

            randomGenerator!.AddValue(randomValue);

            var selected = node.GetSatisfiedNode(gameContext!, gameObject!.GetComponent<PlayerUnit>());
            Assert.That(selected?.name, Is.EqualTo(expectedName));
        }


        public static IEnumerable<TestCaseData> for_weightに応じたNodeが選択されること()
        {
            string name;
            TestCaseData data;

            name = "weightの設定が30,10__ランダムで10を選択した場合__「歩く」を選択";
            data = new TestCaseData(
                10.0f,
                "歩く"
            );
            data.SetName(name);
            yield return data;

            name = "weightの設定が30,10__ランダムで29を選択した場合__「歩く」を選択";
            data = new TestCaseData(
                29.0f,
                "歩く"
            );
            data.SetName(name);
            yield return data;

            name = "weightの設定が30,10__ランダムで29を選択した場合__「周りを見る」を選択";
            data = new TestCaseData(
                30.0f,
                "周りを見る"
            );
            data.SetName(name);
            yield return data;

            name = "weightの設定が30,10__ランダムで39を選択した場合__「周りを見る」を選択";
            data = new TestCaseData(
                39.0f,
                "周りを見る"
            );
            data.SetName(name);
            yield return data;
        }
    }
}
