using UnityEngine;

public class LauncherControllercs : MonoBehaviour
{
    public Transform firePoint; // 発射位置
    public Camera MainCamera;

    void Update()
    {
        if (MainCamera == null) return;

        Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 targetPos = hit.point;
            Vector3 direction = (targetPos - transform.position).normalized;

            // 筒をマウス方向に回転させる（Y軸のみ回転など制限したい場合は調整）
            Quaternion targetRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10f);
        }
    }
}
