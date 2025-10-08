using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{


    // �X�e�[�W�I����ʂֈړ�
    public void GoToStageSelect()
    {
        SceneManager.LoadScene("StageSelect");
    }

    // �X�e�[�W1�֑J��
    public void GoToGameStage1()
    {
        SceneManager.LoadScene("Game1");
    }

    // �X�e�[�W2�֑J��
    public void GoToGameStage2()
    {
        SceneManager.LoadScene("Game2");
    }

    // �X�e�[�W3�֑J��
    public void GoToGameStage3()
    {
        SceneManager.LoadScene("Game3");
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
