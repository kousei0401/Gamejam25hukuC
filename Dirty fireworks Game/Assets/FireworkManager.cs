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
            // �܂��������Ă��Ȃ��ԉ΂�����Δ��˕s��
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

            // FireworkProjectile �Ɏ�����n���āA���������� null �ɂ��Ă��炤
            FireworkProjectile fw = firework.GetComponent<FireworkProjectile>();
            if (fw != null)
            {
                fw.manager = this; // �� ���̃X�N���v�g���g��n��
            }
        }
    }

    // FireworkProjectile ����Ă΂��
    public void OnFireworkExploded(GameObject firework)
    {
        if (currentFirework == firework)
        {
            currentFirework = null;
        }
    }
}
