using UnityEngine;

public class FireworkManager : MonoBehaviour
{
    [Header("花火設定")]
    public GameObject fireworkPrefab;
    public float cooldown = 0.5f;

    [Header("発射位置")]
    public Transform firePoint; // 筒の先端（空の子オブジェクトなど）
    public LayerMask aimMask = ~0; // マウス照準用レイヤー

    private float lastFireTime;
    private GameObject currentFirework = null;

    public bool IsCurrentFirework(GameObject obj) => currentFirework == obj;

    void Update()
    {
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
        Camera cam = Camera.main;
        if (cam == null || fireworkPrefab == null) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Vector3 dir;

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, aimMask))
        {
            dir = (hit.point - firePoint.position).normalized;
        }
        else
        {
            dir = cam.transform.forward;
        }

        GameObject firework = Instantiate(fireworkPrefab, firePoint.position, Quaternion.LookRotation(dir));

        // 現在の花火に登録
        currentFirework = firework;

        // manager参照を渡す
        var proj = firework.GetComponent<FireworkProjectile>();
        if (proj != null) proj.manager = this;
    }

    // Projectile側から呼ばれる
    public void OnFireworkExploded(GameObject firework)
    {
        if (currentFirework == firework)
        {
            currentFirework = null;
        }
    }
}
