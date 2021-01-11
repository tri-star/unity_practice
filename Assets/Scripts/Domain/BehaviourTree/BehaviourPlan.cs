

using ActionSample.Components;

namespace ActionSample.Domain.BehaviourTree
{
    /// <summary>
    /// ビヘイビアツリーの中で最終的に決定される実行プラン
    /// 「X,Y,Zの座標まで歩いて移動する」のような単位の、ある程度まとまった行動の
    /// 実行が完了するまでを制御する
    /// </summary>
    public interface IBehaviourPlan
    {
        string name { get; }

        void Init();

        void Execute(IUnit unit);

        bool isDone();
    }
}
