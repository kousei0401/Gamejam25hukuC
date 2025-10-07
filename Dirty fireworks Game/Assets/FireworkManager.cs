using UnityEngine;

public class FireworkManager : MonoBehaviour
{
    public GameObject fireworkPrefab;
    public float cooldown = 0.5f;

    private float lastFireTime;
    private GameObject currentFirework = null;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time - lastFireTime > cooldown)
        {
            // まだ爆発していない花火があれば発射不可
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
            currentFirework = firework;

            // FireworkProjectile に自分を渡して、爆発したら null にしてもらう
            FireworkProjectile fw = firework.GetComponent<FireworkProjectile>();
            if (fw != null)
            {
                fw.manager = this; // ← このスクリプト自身を渡す
            }
        }
    }

    // FireworkProjectile から呼ばれる
    public void OnFireworkExploded(GameObject firework)
    {
        if (currentFirework == firework)
        {
            currentFirework = null;
        }
    }
}
