#nullable enable

using ActionSample.Components;

namespace ActionSample.Domain.BehaviourTree
{
    public class BehaviourExecutor
    {
        private BehaviourTreeNode _tree;

        private IBehaviourPlan? currentPlan;

        public BehaviourExecutor(BehaviourTreeNode tree)
        {
            _tree = tree;
            currentPlan = null;
        }

        public IBehaviourPlan? Execute(GameContext context, IUnit unit)
        {
            if (!currentPlan?.isDone() ?? false)
            {
                return currentPlan;
            }

            BehaviourTreeNode? cursor = _tree;

            int limitter = 100;

            while (limitter-- > 0)
            {
                if (cursor.HavePlan())
                {
                    currentPlan = cursor.plan;
                    currentPlan!.Init();
                    return currentPlan;
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
