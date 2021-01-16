using UnityEngine;
using static ActionSample.Components.Unit;

namespace ActionSample.Components
{
    public interface IUnit : IComponent
    {
        /// <summary>
        /// 指定方向に歩かせる
        /// </summary>
        /// <param name="x">X方向の移動量</param>
        /// <param name="z">Z方向の移動量</param>
        void MoveToward(float x, float z);

        /// <summary>
        ///指定した方向に力を加える
        /// </summary>
        /// <param name="force">加速度</param>
        void AddForce(Vector3 force);

        /// <summary>
        /// 加速度をゼロにして停止させる
        /// </summary>
        void StopForce();

        /// <summary>
        /// 現在の状態を返す
        /// </summary>
        States GetState();

        /// <summary>
        /// 現在の状態を強制的にセットする
        /// </summary>
        /// <param name="newState"></param>
        void SetState(States newState);

        /// <summary>
        /// 向いている方向
        /// </summary>
        Dimension dimension
        {
            get; set;
        }

        /// <summary>
        /// 現在の加速度
        /// </summary>
        Vector3 velocity
        {
            get; set;
        }

        /// <summary>
        /// ユニットが着地しているかを返す
        /// </summary>
        bool IsGrounded();

        /// <summary>
        /// 指定された状態に遷移可能かを返す
        /// </summary>
        /// <param name="newState">遷移したい状態の値</param>
        /// <returns>状態遷移可能かどうか</returns>
        bool TrySetState(States newState);

        Transform Transform
        { get; }
    }
}
