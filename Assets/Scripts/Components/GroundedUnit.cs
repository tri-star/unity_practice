using UnityEngine;
using ActionSampleCollision = ActionSample.Domain.Collision;

namespace ActionSample.Components
{
    public class GroundedUnit : BaseUnit, IComponent
    {
        private Animator _animator;

        [SerializeField]
        private float _maxGravitySpeed = -3.3f;

        private Collider _collider;

        public void Start()
        {
            this._animator = this.GetComponent<Animator>();
            this._collider = this.GetComponent<Collider>();
            this.velocity = new Vector3(0, 0, 0);
        }

        public new void Update()
        {
            float forceY = 0;
            if (!this.isGrounded)
            {
                forceY = Mathf.Min(this.velocity.y - 0.07f, this._maxGravitySpeed);
            }
            this.velocity = new Vector3(this.velocity.x, forceY, this.velocity.z);
            this._animator.SetBool("isWalking", (Mathf.Abs(this.velocity.x) > 0 || Mathf.Abs(this.velocity.z) > 0));
            //this.transform.localScale = new Vector3(this.velocity.x > 0 ? 1.0f : -1, 1.0f, 1.0f);

            base.Update();
        }


        public void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.tag == "ground")
            {
                this.isGrounded = true;
                adjustFootPosition(collision);
            }
        }

        //専用のサービスに実装を移す
        private void adjustFootPosition(Collision collision)
        {
            Bounds? intersection = ActionSampleCollision.GetIntersection(
                this._collider.bounds,
                collision.collider.bounds
            );

            if(intersection == null)
            {
                return;
            }

            ActionSampleCollision.Dimension? dimension = ActionSampleCollision.GetDimensionFromIntersection(
                this._collider.bounds,
                intersection ?? new Bounds()
            );
            if(dimension != ActionSampleCollision.Dimension.BOTTOM)
            {
                return;
            }

            Debug.Log(this.transform.position.y);
            Debug.Log(intersection?.size.y);
            this.transform.Translate(new Vector3(0, -intersection?.size.y ?? 0, 0));
        }

    }

}
