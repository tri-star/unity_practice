using ActionSample.Parameters;
using ActionSample.Signals;
using UnityEngine;
using Zenject;
using ActionSampleCollision = ActionSample.Domain.Collision;

namespace ActionSample.Components
{
    /// <summary>
    /// ゲーム中で画面に表示されるオブジェクトが持つコンポーネント
    /// 標準では重力の影響を受ける。重力の影響を受けないものはプロパティなどで指定して管理する予定
    /// </summary>
    public class Unit : MonoBehaviour, IComponent
    {

        //@TODO: Domain層で実装することを検討
        public enum States
        {
            NEUTRAL,
            WALKING,
            ATTACK,
            DAMAGE,
        }

        public enum Dimension
        {
            LEFT,
            RIGHT
        }

        [Inject]
        private SignalBus _signalBus;

        private Vector3 _velocity;

        private Animator _animator;

        [Inject]
        private StageSetting _stageSetting;

        private Collider _collider;

        private Collider _groundCollider;

        private States _state;

        [SerializeField]
        private Dimension _dimension;

        [SerializeField]
        protected float speed;

        public void Start()
        {
            this._animator = this.GetComponent<Animator>();
            this._collider = this.GetComponent<Collider>();
            this._velocity = new Vector3(0, 0, 0);
            this._groundCollider = null;
            _state = States.NEUTRAL;
        }

        public void FixedUpdate()
        {
            float forceY = _velocity.y;
            if (!this.IsGrounded())
            {
                forceY = Mathf.Clamp(
                    this._velocity.y + this._stageSetting.gravitySpeed,
                    this._stageSetting.maxGravitySpeed,
                    this._stageSetting.maxGravitySpeed * -1
                );
            }
            else
            {
                forceY = 0;
            }
            this._velocity = new Vector3(this._velocity.x, forceY, this._velocity.z);

            this.transform.Translate(this._velocity);
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
            if (collision.collider.tag == "attack")
            {
                Unit attacker = collision.collider.gameObject.GetComponentInParent<Unit>();
                // @TODO: 攻撃に関する情報は攻撃者のGameObjectから取得する
                _signalBus.Fire<UnitDamageSignal>(new UnitDamageSignal()
                {
                    target = this,
                    damage = 2000,
                    force = new Vector3(2.0f * (attacker.dimension == Dimension.LEFT ? -1 : 1), 20.0f, 0)
                });
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
            if (collision.collider == this._groundCollider)
            {
                this._groundCollider = null;
            }
        }



        public void MoveToward(float x, float z)
        {
            float newX = x * this.speed;
            float newZ = z * this.speed;
            this._velocity = new Vector3(newX, this._velocity.y, newZ);

            if (newX < 0)
            {
                _dimension = Dimension.LEFT;
            }
            else if (newX > 0)
            {
                _dimension = Dimension.RIGHT;
            }
        }

        public void AddForce(Vector3 force)
        {
            _velocity += force;

            if (_velocity.y > 1)
            {
                _groundCollider = null;
            }
        }


        public void StopForce()
        {
            _velocity = new Vector3(0, 0, 0);
        }

        public States GetState()
        {
            return _state;
        }

        public void SetState(States newState)
        {
            States oldState = _state;
            _state = newState;

            _signalBus.Fire<UnitStateChangeSignal>(new UnitStateChangeSignal { oldState = oldState, newState = newState });
        }


        public Dimension dimension
        {
            get { return _dimension; }
            set { _dimension = value; }
        }

        public Vector3 velocity
        {
            get { return _velocity; }
            set { _velocity = value; }
        }

        /// <summary>
        /// ユニットが着地しているかを返す
        /// </summary>
        /// <returns></returns>
        public bool IsGrounded()
        {
            return this._groundCollider == null ? false : true;
        }


        // @TODO: 判定と位置調整の処理は外部のサービスクラスに切り出す
        private void checkGrounded(Collision collision)
        {
            Bounds? intersection = ActionSampleCollision.GetIntersection(
                this._collider.bounds,
                collision.collider.bounds
            );

            if (intersection == null)
            {
                return;
            }

            ActionSampleCollision.Dimension? dimension = ActionSampleCollision.GetDimensionFromIntersection(
                this._collider.bounds,
                intersection ?? new Bounds()
            );
            if (dimension != ActionSampleCollision.Dimension.BOTTOM)
            {
                return;
            }
            this._groundCollider = collision.collider;
            this.transform.Translate(new Vector3(0, intersection?.size.y ?? 0, 0));
        }


        // @TODO: 外部のクラスに移譲する
        private void resolveCollision(Collision collision)
        {
            Bounds? intersection = ActionSampleCollision.GetIntersection(
                this._collider.bounds,
                collision.collider.bounds
            );

            if (intersection == null)
            {
                return;
            }

            ActionSampleCollision.Dimension? dimension = ActionSampleCollision.GetDimensionFromIntersection(
                this._collider.bounds,
                intersection ?? new Bounds()
            );

            float adjustX = 0;
            float adjustZ = 0;
            if (dimension == ActionSampleCollision.Dimension.LEFT)
            {
                adjustX = intersection?.size.x ?? 0;
            }
            if (dimension == ActionSampleCollision.Dimension.RIGHT)
            {
                adjustX = -intersection?.size.x ?? 0;
            }
            if (dimension == ActionSampleCollision.Dimension.FRONT)
            {
                adjustZ = intersection?.size.z ?? 0;
            }
            if (dimension == ActionSampleCollision.Dimension.REAR)
            {
                adjustZ = -intersection?.size.z ?? 0;
            }
            this.transform.Translate(new Vector3(adjustX, 0, adjustZ));
        }

    }

}
