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
            Debug.Log(_rigidbody);
        }

        public void ApplyKnockback(Vector3 knocker)
        {
            Debug.Log("Knockback");
            var direction = (knocker - transform.position).normalized;
            Debug.Log(direction.x <= 0 ? "Left" : "Right");
            var knockVector = knockForce;
            knockVector.x *= -Mathf.Sign(direction.x);
            Debug.Log(knockVector);
            _rigidbody.AddForce(knockVector);
        }
    }
}
