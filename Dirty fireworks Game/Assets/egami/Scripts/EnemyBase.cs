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

    // 各派生クラスで初期化
    protected int m_maxHealth;

    // 基底クラスで初期化
    protected int m_crrentHealth;
    protected EnemyState m_currentState;
    protected SphereCollider m_collider;

    // 誘爆関係
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
            //Debug.LogWarning("初期HPに不正な値が設定されています。");

            m_maxHealth = 1;    // 仮値
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
            Debug.Log("衝突");
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

        Debug.Log("状態が" +  m_currentState + "に変更");
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
