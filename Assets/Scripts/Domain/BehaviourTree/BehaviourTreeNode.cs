#nullable enable

using System.Collections.Generic;
using ActionSample.Components;
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
        public string name { get; private set; }

        /// <summary>
        /// このNodeが実行対象になるための前提条件
        /// </summary>
        public IBehaviourCondition? condition { get; private set; }

        /// <summary>
        /// 自身の重み付け
        /// (値が高いほど、同じ階層の他のノードよりも選ばれる可能性が高くなる)
        /// </summary>
        /// <value></value>
        public int weight { get; private set; }

        /// <summary>
        /// 選択可能なノードの一覧
        /// </summary>
        private List<BehaviourTreeNode> _children;

        /// <summary>
        /// このノードが選択された場合に実行するプラン
        /// </summary>
        public IBehaviourPlan? plan { get; private set; }

        public BehaviourTreeNode(
            string name,
            int weight = 10,
            List<BehaviourTreeNode>? children = null,
            IBehaviourCondition? condition = null,
            IBehaviourPlan? plan = null)
        {
            this.name = name;
            this.weight = weight;
            this.condition = condition;
            this.plan = plan;
            this._children = children == null ? new List<BehaviourTreeNode>() : children;
        }

        public bool isSatisfied(GameContext context, IUnit unit)
        {
            return condition?.isSatisfied(context, unit) ?? true;
        }

        public bool HavePlan()
        {
            return plan != null;
        }

        public void AppendChild(BehaviourTreeNode child)
        {
            _children.Add(child);
        }


        public void AddNodes(BehaviourTreeNode[] children)
        {
            foreach (var child in children)
            {
                _children.Add(child);
            }
        }

        public void SetPlan(IBehaviourPlan plan)
        {
            this.plan = plan;
        }

        /// <summary>
        /// ノード一覧の中から、条件を満たすノードを1つだけ返す。
        /// 条件を満たすものが複数存在した場合、weightに応じてランダムに選択して返す。
        /// </summary>
        /// <param name="unit"></param>
        public BehaviourTreeNode? GetSatisfiedNode(GameContext context, IUnit unit)
        {
            List<BehaviourTreeNode> satisfiedNodes = _children.Where(n => n.isSatisfied(context, unit)).ToList();
            if (satisfiedNodes.Count == 0)
            {
                return null;
            }
            int weightTotal = 0;
            foreach (var child in satisfiedNodes)
            {
                weightTotal += child.weight;
            }

            var randomGenerator = context.RandomGeneratorManager.Get("Game");
            int randomValue = (int)randomGenerator.FromRange(0, weightTotal);
            int currentWeight = 0;
            BehaviourTreeNode? selectedNode = null;

            foreach (var child in _children)
            {
                if (randomValue >= currentWeight && randomValue < (currentWeight + child.weight))
                {
                    selectedNode = child;
                }
                currentWeight += child.weight;
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
