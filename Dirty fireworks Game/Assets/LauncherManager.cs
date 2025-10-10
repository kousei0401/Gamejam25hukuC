using UnityEngine;

public class LauncherManager : MonoBehaviour
{
    [Header("�ݒ�")]
    [Tooltip("Ray���΂��J�����inull�̏ꍇ�̓��C���J�������g�p�j")]
    public Camera targetCamera;

    [Header("�I�v�V����")]
    [Tooltip("��]�̊��炩���i0�ő����ɉ�]�A�傫���قǊ��炩�j")]
    [Range(0f, 20f)]
    public float rotationSmoothness = 5f;

    [Tooltip("���̐�[�̃��[�J���I�t�Z�b�g�i�ʏ��+Y�����j")]
    public Vector3 tubeTopOffset = Vector3.up;

    private void Start()
    {
        // �J�������w�肳��Ă��Ȃ��ꍇ�̓��C���J�������g�p
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }

        if (targetCamera == null)
        {
            Debug.LogError("�J������������܂���I���C���J�����Ƀ^�O��ݒ肷�邩�A�J�������蓮�Ŋ��蓖�ĂĂ��������B");
        }
    }

    private void Update()
    {
        if (targetCamera == null) return;

        // �}�E�X�ʒu����Ray���쐬
        Ray ray = targetCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Ray�������ɓ����������`�F�b�N
        if (Physics.Raycast(ray, out hit))
        {
            // ���̈ʒu���甠�̃q�b�g�ʒu�ւ̕������v�Z
            Vector3 directionToTarget = hit.point - transform.position;

            // �������[���łȂ����Ƃ��m�F
            if (directionToTarget.sqrMagnitude > 0.001f)
            {
                // X-Y���ʂł̊p�x���v�Z�iZ����]�p�j
                float angleZ = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg - 90f;

                // �ڕW�̉�]�iZ���̂ݕύX�AX����Y����0�x�j
                Quaternion targetRotation = Quaternion.Euler(0f, 0f, angleZ);

                // ���炩�ɉ�]
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
