using UnityEngine;
using ActionSampleCollision = ActionSample.Domain.Collision;

namespace ActionSample.Components
{
    /// <summary>
    /// ゲーム中で画面に表示されるオブジェクトが持つコンポーネント
    /// 標準では重力の影響を受ける。重力の影響を受けないものはプロパティなどで指定して管理する予定
    /// </summary>
    public class Unit : MonoBehaviour, IComponent
    {
        private Vector3 _velocity;

        private Animator _animator;

        [SerializeField]
        private float _maxGravitySpeed;

        private Collider _collider;

        private Collider _groundCollider;


        [SerializeField]
        protected float speed;

        public void Start()
        {
            //
            this._animator = this.GetComponent<Animator>();
            this._collider = this.GetComponent<Collider>();
            this._velocity = new Vector3(0, 0, 0);
            this._groundCollider = null;
        }

        public void FixedUpdate()
        {
            float forceY = 0;
            if (!this.IsGrounded())
            {
                // @TODO: 重力加速度はゲーム全体のオプションとして定義する
                forceY = Mathf.Max(this._velocity.y - 0.1f, this._maxGravitySpeed);
            }
            this._velocity = new Vector3(this._velocity.x, forceY, this._velocity.z);

            // @TODO: Player固有のコンポーネントに切り出す
            this._animator.SetBool("isWalking", (Mathf.Abs(this._velocity.x) > 0 || Mathf.Abs(this._velocity.z) > 0));

            // @TODO: 左右反転をシェーダーの中で行う
            //this.transform.localScale = new Vector3(this.velocity.x > 0 ? 1.0f : -1, 1.0f, 1.0f);

            this.transform.Translate(this._velocity);
        }


        public void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.tag == "obstacle")
            {
                resolveCollision(collision);
                checkGrounded(collision);
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
