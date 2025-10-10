using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultManeger : MonoBehaviour
{
    public GameObject[] objects; // �C���X�y�N�^�[�Őݒ�

    void Update()
    {
        if (AnyInactive())
        {
            OnInactiveFound();
        }
    }

    bool AnyInactive()
    {
        foreach (var obj in objects)
        {
            // null�`�F�b�N�{��A�N�e�B�u����
            if (obj != null && !obj.activeSelf)
            {
                return true;
            }
        }
        return false;
    }

    public void OnInactiveFound()
    {
       SceneManager.LoadScene("GamaClear");
    }
}
