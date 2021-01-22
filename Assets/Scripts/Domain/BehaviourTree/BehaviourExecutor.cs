#nullable enable

using ActionSample.Components;

namespace ActionSample.Domain.BehaviourTree
{
    public class BehaviourExecutor
    {
        private BehaviourTreeNode _tree;

        public BehaviourExecutor(BehaviourTreeNode tree)
        {
            _tree = tree;
        }

        public IBehaviourPlan? Execute(GameContext context, IUnit unit)
        {
            BehaviourTreeNode? cursor = _tree;

            int limitter = 100;

            while (limitter-- > 0)
            {
                if (cursor.HavePlan())
                {
                    return cursor.plan;
                }
                cursor = cursor.GetSatisfiedNode(context, unit);
                if (cursor == null)
                {
                    throw new PlanNotFoundException("プランが見つかりませんでした: ");
                }
            }
            return null;
        }
    }
}
