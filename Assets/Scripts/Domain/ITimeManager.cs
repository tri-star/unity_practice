namespace ActionSample.Domain
{
    public interface ITimeManager
    {
        /// <summary>
        /// 前フレームからの経過時間を返す(単位：秒)
        /// </summary>
        float GetDeltaTime();
    }
}
