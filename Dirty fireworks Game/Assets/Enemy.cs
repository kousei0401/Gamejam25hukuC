using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHP = 1;
    private int currentHP;

    void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        Debug.Log($"{gameObject.name} が {damage} ダメージを受けた！ 残りHP: {currentHP}");

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} は倒された！");
        // 死亡時の処理（エフェクトなど追加可）
        Destroy(gameObject);
    }
}
