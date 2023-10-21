using UnityEngine;

namespace System
{
    public class Knockback : MonoBehaviour
    {
        [SerializeField] private Vector2 knockForce;
        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public void ApplyKnockback(Vector3 knocker)
        {
            var direction = (knocker - transform.position).normalized;
            Debug.Log(direction.x <= 0 ? "Left" : "Right");
            var knockVector = knockForce;
            knockVector.x *= -Mathf.Sign(direction.x);
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.AddForce(knockVector, ForceMode2D.Impulse);
        }
    }
}
