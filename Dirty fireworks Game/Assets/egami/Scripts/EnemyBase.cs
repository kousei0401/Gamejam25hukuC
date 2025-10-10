using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public enum EnemyState
    { 
        Idle,
        Exploding,
        Dead,
    }

    // �e�h���N���X�ŏ�����
    protected int m_maxHealth;

    // ���N���X�ŏ�����
    [SerializeField] protected int m_crrentHealth;
    protected EnemyState m_currentState;
    protected SphereCollider m_collider;

    // �A�����j�֌W
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
    /// ����������
    /// </summary>
    protected virtual void Initialize()
    {
        // �^�O���uEnemy�v�ɂ���
        gameObject.tag = "Enemy";

        // �e�h���N���X�ōő�HP�����������Ă�������
        if (m_maxHealth <= 0)
        {
            Debug.LogWarning("����HP�ɕs���Ȓl���ݒ肳��Ă��܂��B");
        }

        // �p�����[�^�̏�����
        m_crrentHealth = m_maxHealth;
        m_currentState = EnemyState.Idle;
        m_collider = GetComponent<SphereCollider>();

        // �A�����j�֘A�̏�����
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
    /// �Փˌ��m
    /// </summary>
    /// <param name="other"></param>
    protected virtual void OnTriggerEnter(Collider other)
    {
        // �uIdle�v��ԈȊO�� return
        if (m_currentState != EnemyState.Idle) return;

        // �ԉ΂ƏՓ�
        if (other.CompareTag("Firework"))
        {
            // �_���[�W
            TakeDamage();
        }
        // �G(�A�����j)�ƏՓ�
        else if(other.CompareTag("Enemy"))
        {
            // �Փ˂����G�̏�Ԋm�F
            EnemyBase enemy = other.GetComponent<EnemyBase>();
            if (enemy == null) return;

            if (enemy.m_currentState == EnemyState.Exploding)
            {
                TakeDamage();
            }
        }
    }


    /// <summary>
    /// �_���[�W���󂯂�
    /// </summary>
    protected virtual void TakeDamage()
    {
        m_crrentHealth--;

        // HP��0�ȉ��ɂȂ����甚�j����
        if (m_crrentHealth <= 0)
        {
            ChangeState(EnemyState.Exploding);
            OnExplosion();
        }
    }


    /// <summary>
    /// ��ԑJ��
    /// </summary>
    /// <param name="nextState"></param>
    protected virtual void ChangeState(EnemyState nextState)
    {
        if (m_currentState == nextState) return;
        m_currentState = nextState;
    }


    /// <summary>
    /// ���j����
    /// </summary>
    protected virtual void OnExplosion()
    {
        m_isExpanding = true;
        m_expamdTimer = 0;

        // �ʏ�e�N�X�`�� -> �ԉ΃e�N�X�`�� �ɐ؂�ւ�

    }


    /// <summary>
    /// �G�̃R���C�_�[�����X�Ɋg��B
    /// �A�����j�̃G�t�F�N�g�ɍ��킹����Ə��ǂ�
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
    /// ���S
    /// </summary>
    protected virtual void OnDead()
    {
        m_collider.radius = m_defaultRadius;
        
        gameObject.SetActive(false);

        // �Ƃ肠�����ʏ��Ԃɖ߂��Ă邯�ǁA����Ȃ�����
        ChangeState(EnemyState.Idle);
    }
}
