using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    // �X�e�[�W�I����ʂֈړ�
    public void GoToStageSelect()
    {
        SceneManager.LoadScene("StageSelect");
    }

    // �Q�[���V�[���ֈړ��i��Ŏg���p�j
    public void GoToGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    // �^�C�g���֖߂�i��Ŏg���p�j
    public void GoToTitle()
    {
        SceneManager.LoadScene("Title");
    }

    // �I�������iPC�r���h���p�j
    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // �G�f�B�^�p
#endif
    }
}
