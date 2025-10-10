using UnityEngine;

public class LauncherController : MonoBehaviour
{
    public Camera mainCamera;

    void Update()
    {
        if (mainCamera == null) return;

        // マウス位置からRayを飛ばす
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        // Rayが何かに当たったら
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // 当たった位置
            Vector3 targetPoint = hit.point;

            // 自分の位置から当たった場所への方向ベクトル
            Vector3 direction = targetPoint - transform.position;

            // 高さ（Y軸）は無視して水平だけ向かせたい場合はコメントアウト外す
            direction.y = 0;

            if (direction.sqrMagnitude > 0.001f)
            {
                // 方向ベクトルから回転を算出
                Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);

                // スムーズに回転
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    lookRotation,
                    Time.deltaTime * 10f
                );
            }
        }
    }
}

