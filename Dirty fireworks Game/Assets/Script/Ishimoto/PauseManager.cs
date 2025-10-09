using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuUI; // ポーズ画面
    [SerializeField] Slider soundSlider;     // サウンドバー

    private bool isPaused = false;

    void Start()
    {
        pauseMenuUI.SetActive(false);

        // 初期音量設定（AudioListenerで全体音量管理）
        soundSlider.value = AudioListener.volume;
        soundSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // ゲーム停止
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // ゲーム再開
        isPaused = false;
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToTitle()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Title");
    }

    void OnVolumeChanged(float value)
    {
        AudioListener.volume = value; // 音量変更
    }
}
