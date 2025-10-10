using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneTransitionManager : MonoBehaviour
{


    // ステージ選択画面へ移動
    public void GoToStageSelect()
    {
        SceneManager.LoadScene("StageSelect");
    }

    // ステージへ遷移
    public void GoToGameStage1()
    {
        SceneManager.LoadScene("Game1");
    }


    // タイトルへ戻る（後で使う用）
    public void GoToTitle()
    {
        SceneManager.LoadScene("Title");
    }

    // ★ ゲームクリア時に呼ばれる関数
    public void OnGameClear()
    {
        SceneManager.LoadScene("GameClear");
    }

    // ★ ゲームオーバー時に呼ばれる関数
    public void OnGameOver()
    {
        SceneManager.LoadScene("GameOver");
    }





    // 終了処理（PCビルド時用）
    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // エディタ用
#endif
    }

    

}















