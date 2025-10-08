using UnityEngine;

public class LauncherControllercs : MonoBehaviour
{
    public Transform firePoint; // ���ˈʒu
    public Camera MainCamera;

    void Update()
    {
        if (MainCamera == null) return;

        Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 targetPos = hit.point;
            Vector3 direction = (targetPos - transform.position).normalized;

            // �����}�E�X�����ɉ�]������iY���̂݉�]�Ȃǐ����������ꍇ�͒����j
            Quaternion targetRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10f);
        }
    }
}
