using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    // ステージ選択画面へ移動
    public void GoToStageSelect()
    {
        SceneManager.LoadScene("StageSelect");
    }

    // ゲームシーンへ移動（後で使う用）
    public void GoToGame()
    {
        SceneManager.LoadScene("GameScene");
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
