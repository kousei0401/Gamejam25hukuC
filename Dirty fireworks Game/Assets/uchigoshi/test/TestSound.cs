using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class TestSound : MonoBehaviour
{

    [SerializeField] private GameTimer gameTimer;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            gameTimer.AddTime(5f);
        }

       if(Input.GetKeyDown(KeyCode.S))
        {
            gameTimer.ReduceTime(5f);
        }
    }

    public void DeleteTest( )
    {
        #if UNITY_EDITOR
             EditorApplication.isPlaying = false;  // 🔹 Unityエディタの実行を止める
#else
        Application.Quit();                   
#endif

    }

}
