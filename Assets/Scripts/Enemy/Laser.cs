using System.Collections;
using UnityEngine;

namespace Enemy
{
    public class Laser : Turret
    {
        private float _laserLifetime;
        
        private float _length;
        [SerializeField] protected LayerMask groundMask;

        protected override void Start()
        {
            RaycastHit2D hit = Physics2D.Raycast(shotPoint.position, transform.right, Mathf.Infinity, groundMask);
            _length = hit.distance;
            Debug.Log(_length);
            
            _laserLifetime = projectile.GetLifetime();
            
            StartCoroutine(Shoot());
        }
        
        protected override IEnumerator Shoot()
        {
            var laser = Instantiate(projectile, shotPoint.position, shotPoint.rotation).transform;

            laser = laser.GetChild(0).transform;
            
            var laserSpriteRenderer = laser.GetComponent<SpriteRenderer>();
            laserSpriteRenderer.size = new Vector2(_length, laserSpriteRenderer.size.y);
            var laserBoxCollider = laser.GetComponent<BoxCollider2D>();
            laserBoxCollider.size = new Vector2(_length, laserBoxCollider.size.y);
            laser.GetComponent<Transform>().localPosition = new Vector3(_length / 2, 0, 0);
            
            yield return new WaitForSeconds(timeBetweenShots + _laserLifetime);
            
            StartCoroutine(Shoot());
        }
    }
}
