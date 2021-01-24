#nullable enable

using System.Collections.Generic;
using ActionSample.Components.Unit;
using System.Linq;

namespace ActionSample.Domain.BehaviourTree
{
    /// <summary>
    /// ビヘイビアツリーのノード
    ///
    /// 行動プランを決定する際、このツリーを辿りながらプランを決定する。
    /// 1. 自身が持っている全ノードの中から、条件を満たすノードを取得する
    /// 2. 条件を満たすノードが複数存在する場合、weight(重み)を意識してランダムに1つ選択する
    /// 3. 選ばれたノードがノードの一覧を持っている場合、(1)を再び繰り返してツリーを下る
    /// 4. ノードがプランを持っている場合、そのプランを採用する
    ///
    /// プランは「X,Y,Zまで歩く」、「攻撃を実行する」ような粒度の行動プランで、
    /// プランが終了すると再びビヘイビアツリーを使って次のプランを決定する。
    /// </summary>
    public class BehaviourTreeNode
    {
        /// <summary>
        ///ノードの名前(日本語。デバッグや定義の可読性のため)。
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// このNodeが実行対象になるための前提条件
        /// </summary>
        public IBehaviourCondition? Condition { get; private set; }

        /// <summary>
        /// 自身の重み付け
        /// (値が高いほど、同じ階層の他のノードよりも選ばれる可能性が高くなる)
        /// </summary>
        /// <value></value>
        public int Weight { get; private set; }

        /// <summary>
        /// 選択可能なノードの一覧
        /// </summary>
        private List<BehaviourTreeNode> children;

        /// <summary>
        /// このノードが選択された場合に実行するプラン
        /// </summary>
        public IBehaviourPlan? Plan { get; private set; }

        public BehaviourTreeNode(
            string name,
            int weight = 10,
            List<BehaviourTreeNode>? children = null,
            IBehaviourCondition? condition = null,
            IBehaviourPlan? plan = null)
        {
            Name = name;
            Weight = weight;
            Condition = condition;
            Plan = plan;
            this.children = children == null ? new List<BehaviourTreeNode>() : children;
        }

        public bool IsSatisfied(GameContext context, IUnit unit)
        {
            return Condition?.IsSatisfied(context, unit) ?? true;
        }

        public bool HavePlan()
        {
            return Plan != null;
        }

        public void AppendChild(BehaviourTreeNode child)
        {
            children.Add(child);
        }


        public void AddNodes(BehaviourTreeNode[] children)
        {
            foreach (var child in children)
            {
                this.children.Add(child);
            }
        }

        public void SetPlan(IBehaviourPlan plan)
        {
            Plan = plan;
        }

        /// <summary>
        /// ノード一覧の中から、条件を満たすノードを1つだけ返す。
        /// 条件を満たすものが複数存在した場合、weightに応じてランダムに選択して返す。
        /// </summary>
        /// <param name="unit"></param>
        public BehaviourTreeNode? GetSatisfiedNode(GameContext context, IUnit unit)
        {
            List<BehaviourTreeNode> satisfiedNodes = children.Where(n => n.IsSatisfied(context, unit)).ToList();
            if (satisfiedNodes.Count == 0)
            {
                return null;
            }
            int weightTotal = 0;
            foreach (var child in satisfiedNodes)
            {
                weightTotal += child.Weight;
            }

            var randomGenerator = context.RandomGeneratorManager.Get("Game");
            int randomValue = (int)randomGenerator.FromRange(0, weightTotal);
            int currentWeight = 0;
            BehaviourTreeNode? selectedNode = null;

            foreach (var child in children)
            {
                if (randomValue >= currentWeight && randomValue < (currentWeight + child.Weight))
                {
                    return child;
                }
                currentWeight += child.Weight;
            }
            return selectedNode;
        }

    }


    public class RootBehaviourTreeNode : BehaviourTreeNode
    {
        public RootBehaviourTreeNode(List<BehaviourTreeNode> children) : base("ルートノード", 0, children, null, null)
        {
        }
    }
}
