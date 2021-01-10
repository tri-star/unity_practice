using UnityEngine;
using static ActionSample.Components.Unit;

namespace ActionSample.Components
{
    public interface IUnit : IComponent
    {
        void MoveToward(float x, float z);

        void AddForce(Vector3 force);
        void StopForce();

        States GetState();
        void SetState(States newState);

        Dimension dimension
        {
            get; set;
        }

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
    }
}
