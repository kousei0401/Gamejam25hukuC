using UnityEngine;

public class FireworkProjectile : MonoBehaviour
{
    public float initialSpeed = 20f;
    public float slowdownRate = 2f;
    public float minSpeed = 0.1f;
    public float explosionRadius = 5f;
    public GameObject explosionEffect;

    public AudioClip launchSE;
    public AudioClip explosionSE;

    [HideInInspector]
    public FireworkManager manager;

    private Rigidbody rb;
    private bool hasExploded = false;
    private AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * initialSpeed;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // ?? 発射時SE（ピッチ変化でバリエーション）
        if (launchSE != null)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(launchSE);
        }
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

        // ?? 爆発SE（ピッチ変化でバリエーション）
        if (explosionSE != null)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(explosionSE);
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
