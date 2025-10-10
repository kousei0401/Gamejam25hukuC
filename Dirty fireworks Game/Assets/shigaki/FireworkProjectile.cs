using UnityEngine;

public class FireworkProjectile : MonoBehaviour
{
    [Header("Movement")]
    public float initialSpeed = 20f;
    public float slowdownRate = 2f;
    public float minSpeed = 0.1f;

    [Header("Explosion")]
    public float explosionRadius = 5f;
    public LayerMask explosionMask = ~0; // �e�����C���[
    public AudioClip launchSE;
    public AudioClip explosionSE;

    [Header("Visual (Child)")]
    [SerializeField] private SpriteSheetPlayer player;   // �q�ɕt����
    [SerializeField] private Sprite[] flightFrames;      // �㏸/�_�΃A�j�� 15���z��̈ꕔ�ł�OK
    [SerializeField] private Sprite[] explosionFrames;   // �����A�j�� 15���z��
    [SerializeField] private float flightFPS = 15f;
    [SerializeField] private float explosionFPS = 24f;

    [HideInInspector] public FireworkManager manager;

    private Rigidbody rb;
    private bool hasExploded = false;
    private AudioSource audioSource;

    void Reset()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        if (!audioSource) audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * initialSpeed;

        if (!audioSource) audioSource = GetComponent<AudioSource>();

        // ����SE
        if (launchSE != null)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(launchSE);
        }

        // ��s���A�j��
        if (player != null && flightFrames != null && flightFrames.Length > 0)
        {
            player.SetFrames(flightFrames, autoPlay: true, resetIndex: true);
            player.FPS = flightFPS;
            player.Loop = true;
        }
    }

    void Update()
    {
        if (!hasExploded && rb.velocity.magnitude > minSpeed)
        {
            rb.velocity -= rb.velocity.normalized * slowdownRate * Time.deltaTime;
        }

        // �E�N���b�N�蓮�N���i���݂̉ԉ΂̂݁j
        if (Input.GetMouseButtonDown(1) && manager != null && manager.IsCurrentFirework(gameObject))
        {
            Explode();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasExploded) return;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            var enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null) enemy.TakeDamage(1);
            Explode();
        }
    }

    public void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        // ����SE
        if (explosionSE != null)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(explosionSE);
        }

        // �����_���[�W
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius, explosionMask);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Firework") && hit.gameObject != gameObject)
            {
                var other = hit.GetComponent<FireworkProjectile>();
                if (other != null) other.Explode();
            }

            if (hit.CompareTag("Enemy"))
            {
                var enemy = hit.GetComponent<Enemy>();
                if (enemy != null) enemy.TakeDamage(1);
            }
        }

        // �����ځF�����A�j���ɐؑ� �� ������Destroy
        if (player != null && explosionFrames != null && explosionFrames.Length > 0)
        {
            player.onFinished.RemoveAllListeners(); // ���d�o�^�h�~
            player.onFinished.AddListener(() =>
            {
                if (manager != null) manager.OnFireworkExploded(gameObject);
                Destroy(gameObject);
            });

            player.SetFrames(explosionFrames, autoPlay: false, resetIndex: true);
            player.FPS = explosionFPS;
            player.Loop = false;
            player.PlayOnce(0);
        }
        else
        {
            // �t�H�[���o�b�N�F�����j��
            if (manager != null) manager.OnFireworkExploded(gameObject);
            Destroy(gameObject);
        }
    }
}
