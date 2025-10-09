using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuUI; // �|�[�Y���
    [SerializeField] Slider soundSlider;     // �T�E���h�o�[

    private bool isPaused = false;

    void Start()
    {
        pauseMenuUI.SetActive(false);

        // �������ʐݒ�iAudioListener�őS�̉��ʊǗ��j
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
        Time.timeScale = 0f; // �Q�[����~
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // �Q�[���ĊJ
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
        AudioListener.volume = value; // ���ʕύX
    }
}
