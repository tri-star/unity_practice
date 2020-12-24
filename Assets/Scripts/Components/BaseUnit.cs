using UnityEngine;


namespace ActionSample.Components
{
    public class BaseUnit : MonoBehaviour, IComponent
    {
        protected bool isGrounded;

        protected Vector3 velocity;

        [SerializeField]
        protected float speed = 0.5f;


        public void Update()
        {
            this.transform.Translate(this.velocity);
        }

        public void MoveToward(float x, float z)
        {
            float newX = Mathf.Clamp(x, -this.speed, this.speed);
            float newZ = Mathf.Clamp(z, -this.speed, this.speed);
            this.velocity = new Vector3(newX, this.velocity.y, newZ);
        }
    }

}
