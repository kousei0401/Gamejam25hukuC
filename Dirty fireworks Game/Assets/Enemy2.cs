using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    public int maxHP = 2;
    private int currentHP;

    void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        Debug.Log($"{gameObject.name} �� {damage} �_���[�W���󂯂��I �c��HP: {currentHP}");

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} �͓|���ꂽ�I");
        // ���S���̏����i�G�t�F�N�g�Ȃǒǉ��j
        Destroy(gameObject);
    }
}
