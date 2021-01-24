using System;
using ActionSample.Domain;
using ActionSample.Parameters;
using ActionSample.Signals;
using UnityEngine;
using Zenject;
using ActionSampleCollision = ActionSample.Domain.Collision;
using Collision = UnityEngine.Collision;

namespace ActionSample.Components.Unit
{
    /// <summary>
    /// ゲーム中で画面に表示されるオブジェクトが持つコンポーネント
    /// 標準では重力の影響を受ける。重力の影響を受けないものはプロパティなどで指定して管理する予定
    /// </summary>
    public class Unit : MonoBehaviour, IUnit, IInitializable, IComponent
    {

        //@TODO: Domain層で実装することを検討
        public enum STATES
        {
            NEUTRAL,
            WALKING,
            ATTACK,
            DAMAGE,
            DEAD,
        }

        public enum DIMENSION
        {
            LEFT,
            RIGHT
        }

        [Inject]
        protected SignalBus signalBus;

        [SerializeField]
        protected bool affectGravity = true;

        protected Vector3 velocity;

        protected Animator animator;

        [Inject]
        protected StageSetting stageSetting;

        protected Collider groundCollider;

        protected STATES state;

        [SerializeField]
        protected DIMENSION _dimension;

        [SerializeField]
        protected float speed;

        public void Initialize()
        {
            this.animator = this.GetComponent<Animator>();
            this.velocity = new Vector3(0, 0, 0);
            this.groundCollider = null;
            state = STATES.NEUTRAL;
        }

        public void FixedUpdate()
        {
            float forceY = velocity.y;

            // @TODO: Gravityコンポーネントに切り出すことを検討
            if (affectGravity)
            {
                if (!this.IsGrounded())
                {
                    forceY = Mathf.Clamp(
                        this.velocity.y + this.stageSetting.gravitySpeed,
                        this.stageSetting.maxGravitySpeed,
                        999
                    );
                }
                else
                {
                    forceY = Mathf.Max(0, forceY);
                }

            }
            this.velocity = new Vector3(this.velocity.x, forceY, this.velocity.z);

            this.transform.Translate(this.velocity);
        }


        public void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.tag == "obstacle")
            {
                resolveCollision(collision);
                checkGrounded(collision);
                if (IsGrounded())
                {
                    StopForce();
                }
            }
        }

        public void OnCollisionStay(Collision collision)
        {
            if (collision.collider.tag == "obstacle")
            {
                resolveCollision(collision);
            }
        }


        public void OnCollisionExit(Collision collision)
        {
            if (collision.collider == this.groundCollider)
            {
                this.groundCollider = null;
            }
        }



        public void MoveToward(float x, float z)
        {
            float newX = x * this.speed;
            float newZ = z * this.speed;
            this.velocity = new Vector3(newX, this.velocity.y, newZ);

            if (newX < 0)
            {
                _dimension = DIMENSION.LEFT;
            }
            else if (newX > 0)
            {
                _dimension = DIMENSION.RIGHT;
            }
        }

        public void AddForce(Vector3 force)
        {
            velocity += force;

            if (velocity.y > 1)
            {
                groundCollider = null;
            }
        }


        public void StopForce()
        {
            velocity = new Vector3(0, 0, 0);
        }

        public STATES GetState()
        {
            return state;
        }

        public void SetState(STATES newState)
        {
            STATES oldState = state;
            state = newState;

            signalBus.Fire<UnitStateChangeSignal>(new UnitStateChangeSignal { oldState = oldState, newState = newState });
            OnChangeState(newState);
        }


        public DIMENSION Dimension
        {
            get { return _dimension; }
            set { _dimension = value; }
        }

        public Vector3 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public bool AffectGravity
        {
            get { return affectGravity; }
            set { affectGravity = value; }
        }

        /// <summary>
        /// ユニットが着地しているかを返す
        /// </summary>
        /// <returns></returns>
        public bool IsGrounded()
        {
            return this.groundCollider == null ? false : true;
        }


        public Transform Transform
        {
            get { return gameObject.transform; }
        }

        public Bounds Bounds
        {
            get { return gameObject.GetComponent<Collider>().bounds; }
        }

        /// <summary>
        /// 指定された状態への遷移が可能かどうかを調べて可能な場合に状態遷移する
        /// </summary>
        /// <param name="newState">遷移したい状態の値</param>
        /// <returns>状態遷移が出来たかどうか</returns>
        public bool TrySetState(Unit.STATES newState)
        {
            if (CanTransitionState(newState))
            {
                SetState(newState);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 指定された状態に遷移可能かを返す
        /// </summary>
        /// <param name="newState">遷移したい状態の値</param>
        /// <returns>状態遷移可能かどうか</returns>
        protected bool CanTransitionState(Unit.STATES newState)
        {
            // @TODO: ドメインに関する実装として切り出す
            switch (GetState())
            {
                case Unit.STATES.NEUTRAL:
                    return true;
                case Unit.STATES.WALKING:
                    return true;
                case Unit.STATES.ATTACK:
                    if (newState == Unit.STATES.WALKING)
                    {
                        return false;
                    }
                    return true;
                case Unit.STATES.DAMAGE:
                    if (newState != Unit.STATES.NEUTRAL)
                    {
                        return false;
                    }
                    return true;
                case Unit.STATES.DEAD:
                    return false;
            }
            throw new NotSupportedException($"無効な状態が指定されました: {GetState()}");
        }

        /// <summary>
        /// 状態が変更された時に呼び出される処理
        /// </summary>
        /// <param name="newState">新しい状態</param>
        protected void OnChangeState(STATES newState)
        {
        }

        /// <summary>
        /// ユニットの状態に応じてAnimatorを更新する
        /// </summary>
        protected void UpdateAnimator()
        {
        }


        // @TODO: 判定と位置調整の処理は外部のサービスクラスに切り出す
        private void checkGrounded(Collision collision)
        {
            Bounds? intersection = ActionSampleCollision.GetIntersection(
                GetComponent<Collider>().bounds,
                collision.collider.bounds
            );

            if (intersection == null)
            {
                return;
            }

            ActionSampleCollision.DIMENSION? dimension = ActionSampleCollision.GetDimensionFromIntersection(
                GetComponent<Collider>().bounds,
                intersection ?? new Bounds()
            );
            if (dimension != ActionSampleCollision.DIMENSION.BOTTOM)
            {
                return;
            }
            this.groundCollider = collision.collider;
            this.transform.Translate(new Vector3(0, intersection?.size.y ?? 0, 0));
        }


        // @TODO: 外部のクラスに移譲する
        private void resolveCollision(Collision collision)
        {
            Bounds? intersection = ActionSampleCollision.GetIntersection(
                GetComponent<Collider>().bounds,
                collision.collider.bounds
            );

            if (intersection == null)
            {
                return;
            }

            ActionSampleCollision.DIMENSION? dimension = ActionSampleCollision.GetDimensionFromIntersection(
                GetComponent<Collider>().bounds,
                intersection ?? new Bounds()
            );

            float adjustX = 0;
            float adjustZ = 0;
            if (dimension == ActionSampleCollision.DIMENSION.LEFT)
            {
                adjustX = intersection?.size.x ?? 0;
            }
            if (dimension == ActionSampleCollision.DIMENSION.RIGHT)
            {
                adjustX = -intersection?.size.x ?? 0;
            }
            if (dimension == ActionSampleCollision.DIMENSION.FRONT)
            {
                adjustZ = intersection?.size.z ?? 0;
            }
            if (dimension == ActionSampleCollision.DIMENSION.REAR)
            {
                adjustZ = -intersection?.size.z ?? 0;
            }
            this.transform.Translate(new Vector3(adjustX, 0, adjustZ));
        }

    }

}
