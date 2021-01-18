using System.Collections.Generic;
using ActionSample.Domain.Behaviour.Conditions;
using ActionSample.Domain.Behaviour.Plans;
using ActionSample.Domain.BehaviourTree;

namespace ActionSample.Domain.Behaviour
{
    public class JImmyBehaviour : IBehaviourTreeBuilder
    {
        public BehaviourTreeNode build()
        {
            return new RootBehaviourTreeNode(new List<BehaviourTreeNode>()
            {
                new BehaviourTreeNode(
                    name: "プレイヤーが遠い",
                    condition: new PlayerXIsGreaterThanEq(40),
                    plan: new TargettingPlan(1.5f)
                ),
                new BehaviourTreeNode(
                    name: "プレイヤーが射程内",
                    condition: new ContactWithPlayer(40),
                    plan: null
                )
            });
        }
    }
}
