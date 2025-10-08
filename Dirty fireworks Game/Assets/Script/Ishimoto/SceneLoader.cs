using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{


    // ステージ選択画面へ移動
    public void GoToStageSelect()
    {
        SceneManager.LoadScene("StageSelect");
    }

    // ステージ1へ遷移
    public void GoToGameStage1()
    {
        SceneManager.LoadScene("Game1");
    }

    // ステージ2へ遷移
    public void GoToGameStage2()
    {
        SceneManager.LoadScene("Game2");
    }

    // ステージ3へ遷移
    public void GoToGameStage3()
    {
        SceneManager.LoadScene("Game3");
    }


    // タイトルへ戻る（後で使う用）
    public void GoToTitle()
    {
        SceneManager.LoadScene("Title");
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
