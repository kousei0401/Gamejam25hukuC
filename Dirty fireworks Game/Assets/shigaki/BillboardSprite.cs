using UnityEngine;

[ExecuteAlways, DisallowMultipleComponent]
public class BillboardSprite : MonoBehaviour
{
    public enum BillboardMode { FullFaceCamera, YAxisOnly }

    [SerializeField] private BillboardMode mode = BillboardMode.FullFaceCamera;
    [SerializeField] private bool lockZRoll = true; // Z‰ñ“]‚ð0‚ÉŒÅ’èi—§‚ÄŠÅ”Â“IŒ©‚¦•ûj

    private Transform tf;

    void Awake() { tf = transform; }

    void LateUpdate()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        if (mode == BillboardMode.FullFaceCamera)
        {
            tf.forward = (tf.position - cam.transform.position).normalized;
        }
        else // YAxisOnlyF…•½‰ñ“]‚Ì‚Ý‚ÅƒJƒƒ‰’Ç]iã‰º‚ÌŽñU‚è‚ð—}‚¦‚éj
        {
            Vector3 dir = tf.position - cam.transform.position;
            dir.y = 0f;
            if (dir.sqrMagnitude > 0.0001f)
                tf.forward = dir.normalized;
        }

        if (lockZRoll)
        {
            Vector3 e = tf.eulerAngles;
            e.z = 0f;
            tf.eulerAngles = e;
        }
    }
}
