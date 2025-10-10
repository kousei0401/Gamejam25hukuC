using UnityEngine;

public class FireworkManager : MonoBehaviour
{
    [Header("�ԉΐݒ�")]
    public GameObject fireworkPrefab;
    public float cooldown = 0.5f;

    [Header("���ˈʒu")]
    public Transform firePoint; // ���̐�[�i��̎q�I�u�W�F�N�g�Ȃǁj
    public LayerMask aimMask = ~0; // �}�E�X�Ə��p���C���[

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

        // ���݂̉ԉ΂ɓo�^
        currentFirework = firework;

        // manager�Q�Ƃ�n��
        var proj = firework.GetComponent<FireworkProjectile>();
        if (proj != null) proj.manager = this;
    }

    // Projectile������Ă΂��
    public void OnFireworkExploded(GameObject firework)
    {
        if (currentFirework == firework)
        {
            currentFirework = null;
        }
    }
}
