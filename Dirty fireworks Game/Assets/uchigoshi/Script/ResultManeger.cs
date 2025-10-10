using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultManeger : MonoBehaviour
{
    public GameObject[] objects; // インスペクターで設定

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
            // nullチェック＋非アクティブ判定
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
