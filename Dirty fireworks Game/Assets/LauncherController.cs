using UnityEngine;

public class LauncherController : MonoBehaviour
{
    public Camera mainCamera;

    void Update()
    {
        if (mainCamera == null) return;

        // �}�E�X�ʒu����Ray���΂�
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        // Ray�������ɓ���������
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // ���������ʒu
            Vector3 targetPoint = hit.point;

            // �����̈ʒu���瓖�������ꏊ�ւ̕����x�N�g��
            Vector3 direction = targetPoint - transform.position;

            // �����iY���j�͖������Đ������������������ꍇ�̓R�����g�A�E�g�O��
            direction.y = 0;

            if (direction.sqrMagnitude > 0.001f)
            {
                // �����x�N�g�������]���Z�o
                Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);

                // �X���[�Y�ɉ�]
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    lookRotation,
                    Time.deltaTime * 10f
                );
            }
        }
    }
}

