using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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
    protected int m_crrentHealth;
    protected EnemyState m_currentState;
    protected SphereCollider m_collider;

    // �U���֌W
    protected float m_defaultRadius;
    protected float m_explosionRadiusMultiplier;
    protected float m_explosionExpandTime;
    protected float m_expamdTimer;
    protected bool m_isExpanding;


    protected virtual void Start()
    {
        Initialize();
    }


    protected virtual void Initialize()
    {
        gameObject.tag = "Enemy";

        if (m_maxHealth <= 0)
        {
            //Debug.LogWarning("����HP�ɕs���Ȓl���ݒ肳��Ă��܂��B");

            m_maxHealth = 1;    // ���l
        }

        m_crrentHealth = m_maxHealth;
        m_currentState = EnemyState.Idle;
        m_collider = GetComponent<SphereCollider>();

        m_defaultRadius = m_collider.radius;
        m_explosionRadiusMultiplier = 1.5f;
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


    protected virtual void OnTriggerEnter(Collider other)
    {
        if (m_currentState != EnemyState.Idle) return;

        if (other.CompareTag("Firework"))
        {
            Debug.Log("�Փ�");
            TakeDamage();
        }
        else if(other.CompareTag("Enemy"))
        {
            EnemyBase enemy = other.GetComponent<EnemyBase>();
            if (enemy == null) return;

            if (enemy.m_currentState == EnemyState.Exploding)
            {
                TakeDamage();
            }
        }
    }


    protected virtual void TakeDamage()
    {
        m_crrentHealth--;

        if (m_crrentHealth <= 0)
        {
            ChangeState(EnemyState.Exploding);
            OnExplosion();
        }
    }


    protected virtual void ChangeState(EnemyState nextState)
    {
        if (m_currentState == nextState) return;
        m_currentState = nextState;

        Debug.Log("��Ԃ�" +  m_currentState + "�ɕύX");
    }


    protected virtual void OnExplosion()
    {
        m_isExpanding = true;
        m_expamdTimer = 0;
    }


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


    protected virtual void OnDead()
    {
        m_collider.radius = m_defaultRadius;

        gameObject.SetActive(false);
    }
}
