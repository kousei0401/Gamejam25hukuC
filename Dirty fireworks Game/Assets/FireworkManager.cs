using UnityEngine;

public class FireworkManager : MonoBehaviour
{
    [Header("�ԉΐݒ�")]
    public GameObject fireworkPrefab;
    public float cooldown = 0.5f;

    public bool IsCurrentFirework(GameObject obj)
    {
        return currentFirework == obj;
    }


    [Header("���ˈʒu")]
    public Transform firePoint; // ���̐�[�i��̎q�I�u�W�F�N�g�Ȃǁj

    private float lastFireTime;
    private GameObject currentFirework = null;

    void Update()
    {
        // ���N���b�N + �N�[���^�C�� + �ԉ΂��܂������ꍇ�̂ݔ���
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

    // �ԉ΂������������ɌĂяo�����iProjectile������j
    public void OnFireworkExploded(GameObject firework)
    {
        if (currentFirework == firework)
        {
            currentFirework = null;
        }
    }
}
