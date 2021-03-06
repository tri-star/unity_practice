namespace ActionSample.Domain.BehaviourTree
{
    public interface IBehaviourCondition
    {
        /// <summary>
        /// 条件を満たしたかどうかの判定を行う
        /// </summary>
        /// <param name="unit">判定の対象になるユニット</param>
        /// <returns>条件を満たしたかどうか</returns>
        bool IsSatisfied(GameContext context, Components.Unit.IUnit unit);
    }
}
