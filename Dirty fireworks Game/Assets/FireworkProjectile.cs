using System.Collections.Generic;
using UnityEngine;

public class FireworkProjectile : MonoBehaviour
{
    public float explosionRadius = 5f;
    public GameObject explosionEffect;
    public float initialSpeed = 20f;

    public FireworkManager manager; // �� ���ˌ��}�l�[�W���[�i�ʒm�p�j

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

        // �A������
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

        // ������ʒm���Ă���폜
        if (manager != null)
        {
            manager.OnFireworkExploded(gameObject);
        }

        Destroy(gameObject);
    }
}
