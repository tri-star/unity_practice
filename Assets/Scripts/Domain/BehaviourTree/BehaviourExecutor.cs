#nullable enable

using ActionSample.Components;

namespace ActionSample.Domain.BehaviourTree
{
    public class BehaviourExecutor
    {
        private BehaviourTreeNode _tree;

        BehaviourExecutor(BehaviourTreeNode tree)
        {
            _tree = tree;
        }

        public IBehaviourPlan? Execute(IUnit unit)
        {
            BehaviourTreeNode? cursor = _tree;

            int limitter = 100;

            while (limitter-- > 0)
            {
                if (cursor.HavePlan())
                {
                    return cursor.plan;
                }
                cursor = cursor.GetSatisfiedNode(unit);
                if (cursor == null)
                {
                    throw new PlanNotFoundException("プランが見つかりませんでした: ");
                }
            }
            return null;
        }
    }
}
