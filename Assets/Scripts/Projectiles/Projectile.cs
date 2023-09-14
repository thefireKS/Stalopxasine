using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [SerializeField] protected float seconds;

    protected Animator _animator;
    protected Rigidbody2D _rb2d;

    private void OnEnable()
    {
        _animator = GetComponentInChildren<Animator>();
        _rb2d = GetComponent<Rigidbody2D>();

        float angle = transform.eulerAngles.z % 5 == 0 ? 0f : 1f;

        _animator?.SetFloat("Angle", angle);

        Destroy(gameObject,seconds);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.name + " // " + other.tag);

        //if (other.TryGetComponent<>)
        //logic

        if (other.CompareTag("Ground"))
            Destroy(gameObject);
    }
}
