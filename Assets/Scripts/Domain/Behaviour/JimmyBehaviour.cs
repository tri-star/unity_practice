using System.Collections.Generic;
using ActionSample.Domain.Behaviour.Conditions;
using ActionSample.Domain.Behaviour.Plans;
using ActionSample.Domain.BehaviourTree;

namespace ActionSample.Domain.Behaviour
{
    public class JimmyBehaviour : IBehaviourTreeBuilder
    {
        public BehaviourTreeNode Build()
        {
            return new RootBehaviourTreeNode(new List<BehaviourTreeNode>()
            {
                new BehaviourTreeNode(
                    name: "プレイヤーが遠い",
                    condition: new PlayerXIsGreaterThanEq(40),
                    children: new List<BehaviourTreeNode>() {
                        new BehaviourTreeNode(
                            name: "プレイヤーに近づく",
                            weight: 30,
                            plan: new TargettingPlan(1.0f)
                        ),
                        new BehaviourTreeNode(
                            name: "何もしない",
                            weight: 10,
                            plan: new NoOpPlan(0.8f)
                        ),
                    }
                ),
                new BehaviourTreeNode(
                    name: "プレイヤーが射程内",
                    condition: new ContactWithPlayer(60),
                    children: new List<BehaviourTreeNode>() {
                        new BehaviourTreeNode(
                            name: "攻撃する",
                            weight: 50,
                            plan: new MeleePlan()
                        ),
                        new BehaviourTreeNode(
                            name: "何もしない",
                            weight: 10,
                            plan: new NoOpPlan(0.8f)
                        ),
                    }
                )
            });
        }
    }
}
