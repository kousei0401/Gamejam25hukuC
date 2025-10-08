
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject pauseMenuUI;   // ポーズメニュー全体
    [SerializeField] private Slider masterVolumeSlider; // 音量調整スライダー

    private bool isPaused = false;

    void Start()
    {
        // 開始時はメニューを非表示
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        // スライダーの初期値を現在の音量に合わせる
        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.value = AudioListener.volume;
            masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        }
    }

    void Update()
    {
        // 🔹 EscキーでポーズON/OFF
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    // 🔹 ポーズする
    public void PauseGame()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(true);

        Time.timeScale = 0f; // ゲーム内時間を止める
        isPaused = true;
    }

    // 🔹 ゲーム再開
    public void ResumeGame()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        Time.timeScale = 1f; // 再開
        isPaused = false;
    }

    // 🔹 リトライ（現在のシーンを再読み込み）
    public void RetryGame()
    {
        Time.timeScale = 1f; // 念のため戻してから
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // 🔹 タイトルに戻る（タイトルシーン名を指定）
    public void ReturnToTitle()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Title");
    }

    // 🔹 音量調整
    public void SetMasterVolume(float volume)
    {
        AudioListener.volume = volume; // 0〜1の範囲で音量を調整
    }

    // 🔹 ゲームを終了（エディタ・ビルド両対応）
    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

