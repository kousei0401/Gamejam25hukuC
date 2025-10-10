using UnityEngine;

public class LauncherManager : MonoBehaviour
{
    [Header("設定")]
    [Tooltip("Rayを飛ばすカメラ（nullの場合はメインカメラを使用）")]
    public Camera targetCamera;

    [Header("オプション")]
    [Tooltip("回転の滑らかさ（0で即座に回転、大きいほど滑らか）")]
    [Range(0f, 20f)]
    public float rotationSmoothness = 5f;

    [Tooltip("筒の先端のローカルオフセット（通常は+Y方向）")]
    public Vector3 tubeTopOffset = Vector3.up;

    private void Start()
    {
        // カメラが指定されていない場合はメインカメラを使用
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }

        if (targetCamera == null)
        {
            Debug.LogError("カメラが見つかりません！メインカメラにタグを設定するか、カメラを手動で割り当ててください。");
        }
    }

    private void Update()
    {
        if (targetCamera == null) return;

        // マウス位置からRayを作成
        Ray ray = targetCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Rayが何かに当たったかチェック
        if (Physics.Raycast(ray, out hit))
        {
            // 筒の位置から箱のヒット位置への方向を計算
            Vector3 directionToTarget = hit.point - transform.position;

            // 方向がゼロでないことを確認
            if (directionToTarget.sqrMagnitude > 0.001f)
            {
                // X-Y平面での角度を計算（Z軸回転用）
                float angleZ = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg - 90f;

                // 目標の回転（Z軸のみ変更、X軸とY軸は0度）
                Quaternion targetRotation = Quaternion.Euler(0f, 0f, angleZ);

                // 滑らかに回転
                if (rotationSmoothness > 0)
                {
                    transform.rotation = Quaternion.Slerp(
                        transform.rotation,
                        targetRotation,
                        Time.deltaTime * rotationSmoothness
                    );
                }
                else
                {
                    transform.rotation = targetRotation;
                }
            }
        }
    }
}
