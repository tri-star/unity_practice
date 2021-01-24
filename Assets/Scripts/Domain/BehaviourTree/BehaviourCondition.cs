using System.Collections.Generic;
using ActionSample.Components.Unit;

namespace ActionSample.Domain.BehaviourTree
{
    /// <summary>
    /// 前提条件を満たしているかの判定を行うためのクラス
    ///
    /// サブクラスで実際の判定条件を実装する
    /// </summary>
    public abstract class BehaviourCondition : IBehaviourCondition
    {
        /// <see cref="IBehaviourCondition"/>
        public abstract bool IsSatisfied(GameContext context, Components.Unit.IUnit unit);

        /// <summary>
        /// 渡された条件全てをANDで判定するConditionを返すファクトリメソッド
        /// </summary>
        /// <param name="conditions">条件一覧</param>
        public static IBehaviourCondition And(List<IBehaviourCondition> conditions)
        {
            return new BehaviourConditionAnd(conditions);
        }

        /// <summary>
        /// 渡された条件全てをORで判定するConditionを返すファクトリメソッド
        /// </summary>
        /// <param name="conditions">条件一覧</param>
        public static IBehaviourCondition Or(List<IBehaviourCondition> conditions)
        {
            return new BehaviourConditionOr(conditions);
        }

        /// <summary>
        /// 渡された条件をNOTで判定するConditionを返すファクトリメソッド
        /// </summary>
        /// <param name="conditions">条件一覧</param>
        public static IBehaviourCondition Not(IBehaviourCondition condition)
        {
            return new BehaviourConditionNot(condition);
        }


        /// <summary>
        /// 渡された条件が全てtrueかどうかを判定するクラス
        /// </summary>
        class BehaviourConditionAnd : IBehaviourCondition
        {
            private List<IBehaviourCondition> conditions;

            public BehaviourConditionAnd(List<IBehaviourCondition> conditions)
            {
                this.conditions = conditions;
            }

            public bool IsSatisfied(GameContext context, IUnit unit)
            {
                bool isAllSatisfied = true;

                foreach (var condition in conditions)
                {
                    if (!condition.IsSatisfied(context, unit))
                    {
                        isAllSatisfied = false;
                    }
                }
                return isAllSatisfied;
            }
        }


        /// <summary>
        /// 渡された条件のどれかがtrueかどうかを判定するクラス
        /// </summary>
        class BehaviourConditionOr : IBehaviourCondition
        {
            private List<IBehaviourCondition> conditions;

            public BehaviourConditionOr(List<IBehaviourCondition> conditions)
            {
                this.conditions = conditions;
            }

            public bool IsSatisfied(GameContext context, IUnit unit)
            {
                bool isSatisfied = false;

                if (conditions.Count == 0)
                {
                    return true;
                }

                foreach (var condition in conditions)
                {
                    if (condition.IsSatisfied(context, unit))
                    {
                        isSatisfied = true;
                        break;
                    }
                }
                return isSatisfied;
            }
        }


        /// <summary>
        /// 渡された条件のどれかがtrueかどうかを判定するクラス
        /// </summary>
        class BehaviourConditionNot : IBehaviourCondition
        {
            private IBehaviourCondition condition;

            public BehaviourConditionNot(IBehaviourCondition condition)
            {
                this.condition = condition;
            }

            public bool IsSatisfied(GameContext context, IUnit unit)
            {
                return !condition.IsSatisfied(context, unit);
            }
        }
    }
}
