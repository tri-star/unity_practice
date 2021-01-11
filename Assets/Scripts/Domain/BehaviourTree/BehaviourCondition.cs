using System.Collections.Generic;
using ActionSample.Components;

namespace ActionSample.Domain.BehaviourTree
{
    public interface IBehaviourCondition
    {
        /// <summary>
        /// 条件を満たしたかどうかの判定を行う
        /// </summary>
        /// <param name="unit">判定の対象になるユニット</param>
        /// <returns>条件を満たしたかどうか</returns>
        bool isSatisfied(Components.IUnit unit);
    }

    /// <summary>
    /// 前提条件を満たしているかの判定を行うためのクラス
    ///
    /// サブクラスで実際の判定条件を実装する
    /// </summary>
    public abstract class BehaviourCondition : IBehaviourCondition
    {
        /// <see cref="IBehaviourCondition"/>
        public abstract bool isSatisfied(Components.IUnit unit);

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
    }


    /// <summary>
    /// 渡された条件が全てtrueかどうかを判定するクラス
    /// </summary>
    public class BehaviourConditionAnd : IBehaviourCondition
    {
        private List<IBehaviourCondition> _conditions;

        public BehaviourConditionAnd(List<IBehaviourCondition> conditions)
        {
            _conditions = conditions;
        }

        public bool isSatisfied(IUnit unit)
        {
            bool isAllSatisfied = true;

            foreach (var condition in _conditions)
            {
                if (!condition.isSatisfied(unit))
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
    public class BehaviourConditionOr : IBehaviourCondition
    {
        private List<IBehaviourCondition> _conditions;

        public BehaviourConditionOr(List<IBehaviourCondition> conditions)
        {
            _conditions = conditions;
        }

        public bool isSatisfied(IUnit unit)
        {
            bool isSatisfied = false;

            if (_conditions.Count == 0)
            {
                return true;
            }

            foreach (var condition in _conditions)
            {
                if (condition.isSatisfied(unit))
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
    public class BehaviourConditionNot : IBehaviourCondition
    {
        private IBehaviourCondition _condition;

        public BehaviourConditionNot(IBehaviourCondition condition)
        {
            _condition = condition;
        }

        public bool isSatisfied(IUnit unit)
        {
            return !_condition.isSatisfied(unit);
        }
    }
}
