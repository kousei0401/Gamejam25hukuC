using UnityEngine;

public class FireworkProjectile : MonoBehaviour
{
    public float initialSpeed = 20f;
    public float slowdownRate = 2f;
    public float minSpeed = 0.1f;
    public float explosionRadius = 5f;
    public GameObject explosionEffect;

    [HideInInspector]
    public FireworkManager manager;

    private Rigidbody rb;
    private bool hasExploded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * initialSpeed;
    }

    void Update()
    {
        if (!hasExploded && rb.velocity.magnitude > minSpeed)
        {
            rb.velocity -= rb.velocity.normalized * slowdownRate * Time.deltaTime;
        }

        if (Input.GetMouseButtonDown(1) && manager != null && manager.IsCurrentFirework(gameObject))
        {
            Explode();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasExploded) return;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(1);
            }
            Explode();
        }
    }

    public void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Firework") && hit.gameObject != gameObject)
            {
                FireworkProjectile other = hit.GetComponent<FireworkProjectile>();
                if (other != null)
                {
                    other.Explode();
                }
            }

            if (hit.CompareTag("Enemy"))
            {
                Enemy enemy = hit.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(1);
                }
            }
        }

        if (manager != null)
        {
            manager.OnFireworkExploded(gameObject);
        }

        Destroy(gameObject);
    }
}
