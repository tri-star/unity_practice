using UnityEngine;
using static ActionSample.Components.Unit.Unit;

namespace ActionSample.Components.Unit
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
        STATES GetState();

        /// <summary>
        /// 現在の状態を強制的にセットする
        /// </summary>
        /// <param name="newState"></param>
        void SetState(STATES newState);

        /// <summary>
        /// 向いている方向
        /// </summary>
        DIMENSION Dimension
        {
            get; set;
        }

        /// <summary>
        /// 現在の加速度
        /// </summary>
        Vector3 Velocity
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
        bool TrySetState(STATES newState);

        /// <summary>
        /// ユニットが属しているGameObjectの他のコンポーネントを取得して返す
        /// </summary>
        /// <param name="component">取得したコンポーネント</param>
        /// <typeparam name="T">取得したいコンポーネントの型</typeparam>
        /// <returns>取得できたかどうか</returns>
        bool TryGetComponent<T>(out T component);

        /// <summary>
        /// ユニットが属しているGameObjectの他のコンポーネントを取得して返す
        /// </summary>
        /// <typeparam name="T">取得したいコンポーネントの型</typeparam>
        /// <returns>取得したコンポーネント</returns>
        T GetComponent<T>();

        Transform Transform
        { get; }

        Bounds Bounds
        { get; }
    }
}
