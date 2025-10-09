using UnityEngine;

public class LauncherController : MonoBehaviour
{
    public Camera mainCamera;

    void Update()
    {
        if (mainCamera == null) return;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 targetPoint = hit.point;

            //  �������܂߂������𐳂����v�Z
            Vector3 direction = (targetPoint - transform.position).normalized;

            //  ���S��3D��]�Ō�������
            Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        }
    }
}
