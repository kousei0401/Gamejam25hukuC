using System.Collections.Generic;
using UnityEngine;

public class FireworkProjectile : MonoBehaviour
{
    public float explosionRadius = 5f;
    public GameObject explosionEffect;
    public float initialSpeed = 20f;

    public FireworkManager manager; // ← 発射元マネージャー（通知用）

    private static HashSet<GameObject> exploded = new HashSet<GameObject>();

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = transform.forward * initialSpeed;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (exploded.Contains(gameObject)) return;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Explode();
        }
    }

    public void Explode()
    {
        if (exploded.Contains(gameObject)) return;

        exploded.Add(gameObject);

        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // 連鎖爆発
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var col in hitColliders)
        {
            GameObject obj = col.gameObject;

            if (obj.CompareTag("Firework") && obj != this.gameObject)
            {
                FireworkProjectile other = obj.GetComponent<FireworkProjectile>();
                if (other != null)
                {
                    other.Explode();
                }
            }
        }

        // 爆発を通知してから削除
        if (manager != null)
        {
            manager.OnFireworkExploded(gameObject);
        }

        Destroy(gameObject);
    }
}
