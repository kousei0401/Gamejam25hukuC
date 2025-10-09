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

            //  ‚‚³‚àŠÜ‚ß‚½•ûŒü‚ğ³‚µ‚­ŒvZ
            Vector3 direction = (targetPoint - transform.position).normalized;

            //  Š®‘S‚È3D‰ñ“]‚ÅŒü‚©‚¹‚é
            Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        }
    }
}
