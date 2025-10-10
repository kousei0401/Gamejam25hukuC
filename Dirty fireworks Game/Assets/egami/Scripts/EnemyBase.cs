using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public enum EnemyState
    { 
        Idle,
        Exploding,
        Dead,
    }

    // 各派生クラスで初期化
    protected int m_maxHealth;

    // 基底クラスで初期化
    [SerializeField] protected int m_crrentHealth;
    protected EnemyState m_currentState;
    protected SphereCollider m_collider;

    // 連鎖爆破関係
    protected float m_defaultRadius;
    protected float m_explosionRadiusMultiplier;
    protected float m_explosionExpandTime;
    protected float m_expamdTimer;
    protected bool m_isExpanding;


    protected virtual void Start()
    {
        Initialize();
    }


    /// <summary>
    /// 初期化処理
    /// </summary>
    protected virtual void Initialize()
    {
        // タグを「Enemy」にする
        gameObject.tag = "Enemy";

        // 各派生クラスで最大HPを初期化してください
        if (m_maxHealth <= 0)
        {
            Debug.LogWarning("初期HPに不正な値が設定されています。");
        }

        // パラメータの初期化
        m_crrentHealth = m_maxHealth;
        m_currentState = EnemyState.Idle;
        m_collider = GetComponent<SphereCollider>();

        // 連鎖爆破関連の初期化
        m_defaultRadius = m_collider.radius;
        m_explosionRadiusMultiplier = 2.01f;
        m_explosionExpandTime = 1.5f;
        m_expamdTimer = 0;
        m_isExpanding = false;
    }


    protected void Update()
    {
        if (m_isExpanding)
        {
            ExpandColliderOverTime();
        }
    }


    /// <summary>
    /// 衝突検知
    /// </summary>
    /// <param name="other"></param>
    protected virtual void OnTriggerEnter(Collider other)
    {
        // 「Idle」状態以外は return
        if (m_currentState != EnemyState.Idle) return;

        // 花火と衝突
        if (other.CompareTag("Firework"))
        {
            // ダメージ
            TakeDamage();
        }
        // 敵(連鎖爆破)と衝突
        else if(other.CompareTag("Enemy"))
        {
            // 衝突した敵の状態確認
            EnemyBase enemy = other.GetComponent<EnemyBase>();
            if (enemy == null) return;

            if (enemy.m_currentState == EnemyState.Exploding)
            {
                TakeDamage();
            }
        }
    }


    /// <summary>
    /// ダメージを受ける
    /// </summary>
    protected virtual void TakeDamage()
    {
        m_crrentHealth--;

        // HPが0以下になったら爆破する
        if (m_crrentHealth <= 0)
        {
            ChangeState(EnemyState.Exploding);
            OnExplosion();
        }
    }


    /// <summary>
    /// 状態遷移
    /// </summary>
    /// <param name="nextState"></param>
    protected virtual void ChangeState(EnemyState nextState)
    {
        if (m_currentState == nextState) return;
        m_currentState = nextState;
    }


    /// <summary>
    /// 爆破処理
    /// </summary>
    protected virtual void OnExplosion()
    {
        m_isExpanding = true;
        m_expamdTimer = 0;

        // 通常テクスチャ -> 花火テクスチャ に切り替え

    }


    /// <summary>
    /// 敵のコライダーを徐々に拡大。
    /// 連鎖爆破のエフェクトに合わせられると尚良し
    /// </summary>
    private void ExpandColliderOverTime()
    {
        m_expamdTimer += Time.deltaTime;
        float t = Mathf.Clamp01(m_expamdTimer / m_explosionExpandTime);

        t = 1f - Mathf.Pow(1f - t, 3f);

        m_collider.radius = Mathf.Lerp(0f, m_defaultRadius * m_explosionRadiusMultiplier, t);

        if (t >= 1f)
        {
            m_isExpanding = false;
            ChangeState(EnemyState.Dead);
            OnDead();
        }
    }


    /// <summary>
    /// 死亡
    /// </summary>
    protected virtual void OnDead()
    {
        m_collider.radius = m_defaultRadius;
        
        gameObject.SetActive(false);

        // とりあえず通常状態に戻してるけど、いらないかも
        ChangeState(EnemyState.Idle);
    }
}
