using UnityEngine;

public class FireworkManager : MonoBehaviour
{
    [Header("花火設定")]
    public GameObject fireworkPrefab;
    public float cooldown = 0.5f;

    public bool IsCurrentFirework(GameObject obj)
    {
        return currentFirework == obj;
    }


    [Header("発射位置")]
    public Transform firePoint; // 筒の先端（空の子オブジェクトなど）

    private float lastFireTime;
    private GameObject currentFirework = null;

    void Update()
    {
        // 左クリック + クールタイム + 花火がまだ無い場合のみ発射
        if (Input.GetMouseButtonDown(0) && Time.time - lastFireTime > cooldown)
        {
            if (currentFirework == null)
            {
                lastFireTime = Time.time;
                LaunchFirework();
            }
        }
    }


    void LaunchFirework()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 direction = (hit.point - transform.position).normalized;

            GameObject firework = Instantiate(fireworkPrefab, transform.position, Quaternion.LookRotation(direction));
        }
    }

    // 花火が爆発した時に呼び出される（Projectile側から）
    public void OnFireworkExploded(GameObject firework)
    {
        if (currentFirework == firework)
        {
            currentFirework = null;
        }
    }
}
