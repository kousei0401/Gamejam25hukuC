using UnityEngine;

public class FireworkProjectile : MonoBehaviour
{
    [Header("Movement")]
    public float initialSpeed = 20f;
    public float slowdownRate = 2f;
    public float minSpeed = 0.1f;

    [Header("Explosion")]
    public float explosionRadius = 5f;
    public LayerMask explosionMask = ~0; // 影響レイヤー
    public AudioClip launchSE;
    public AudioClip explosionSE;

    [Header("Visual (Child)")]
    [SerializeField] private SpriteSheetPlayer player;   // 子に付ける
    [SerializeField] private Sprite[] flightFrames;      // 上昇/点火アニメ 15枚想定の一部でもOK
    [SerializeField] private Sprite[] explosionFrames;   // 爆発アニメ 15枚想定
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

        // 発射SE
        if (launchSE != null)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(launchSE);
        }

        // 飛行中アニメ
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

        // 右クリック手動起爆（現在の花火のみ）
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

        // 爆発SE
        if (explosionSE != null)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(explosionSE);
        }

        // 爆風ダメージ
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

        // 見た目：爆発アニメに切替 → 完了時Destroy
        if (player != null && explosionFrames != null && explosionFrames.Length > 0)
        {
            player.onFinished.RemoveAllListeners(); // 多重登録防止
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
            // フォールバック：即時破棄
            if (manager != null) manager.OnFireworkExploded(gameObject);
            Destroy(gameObject);
        }
    }
}
