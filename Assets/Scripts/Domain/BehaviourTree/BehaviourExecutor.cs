#nullable enable

using ActionSample.Components.Unit;

namespace ActionSample.Domain.BehaviourTree
{
    public class BehaviourExecutor
    {
        private BehaviourTreeNode tree;

        private IBehaviourPlan? currentPlan;

        public BehaviourExecutor(BehaviourTreeNode tree)
        {
            this.tree = tree;
            currentPlan = null;
        }

        public IBehaviourPlan? Execute(GameContext context, IUnit unit)
        {
            if (!currentPlan?.IsDone() ?? false)
            {
                return currentPlan;
            }

            BehaviourTreeNode? cursor = tree;

            int limitter = 100;

            while (limitter-- > 0)
            {
                if (cursor.HavePlan())
                {
                    currentPlan = cursor.Plan;
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
