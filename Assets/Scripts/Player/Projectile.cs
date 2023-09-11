using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public float seconds;

    private Animator _animator;

    private void OnEnable()
    {
        Time.timeScale = 0.1f;
        Debug.Log(transform.eulerAngles.z);
        _animator = GetComponentInChildren<Animator>();
        _animator?.SetFloat("Angle", transform.eulerAngles.z % 5 == 0 ? 0 : 1);
        Destroy(gameObject,seconds);
    }

    private void Update()
    {
        if(speed == 0f) return;
        transform.position += transform.right * (speed * Time.deltaTime);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
            Destroy(gameObject);
    }
}
