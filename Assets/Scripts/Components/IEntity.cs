using ActionSample.Domain;

namespace ActionSample.Components
{
    /// <summary>
    /// ゲーム中に登場する全てのオブジェクトが持つ想定のコンポーネント
    /// (画面に表示されるもの、非表示で他のGameObjectの管理だけ行うものなど)
    /// </summary>
    public interface IEntity : IComponent
    {
        int Id { get; }

        void EntityFixedUpdate(GameContext context);
    }
}

