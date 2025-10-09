using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �Q�[���S�̂̃T�E���h�iBGM�ESE�j�𓝈�Ǘ�����N���X
/// ���̃X�N���v�g����́uSoundManager.Instance�v�ŃA�N�Z�X�\
/// �� �FSoundManager.Instance.PlayBGM(SoundManager.BGMType.BattleBGM);
/// </summary>
public class SoundManager : MonoBehaviour
{
    // ---- Singleton ----
    public static SoundManager Instance { get; private set; }

    // ---- Enum ----
    public enum BGMType
    {
        TitleBGM,
        BattleBGM,
        GameOverBGM,
        ClearBGM,
    }

    public enum SEType
    {
        BombSE,
    }

    // ---- Inspector�ݒ� ----
    [Header("BGM Clips (Enum�̏��ɐݒ肵�Ă�������)")]
    [SerializeField] private AudioClip[] m_bgmClips;

    [Header("SE Clips (Enum�̏��ɐݒ肵�Ă�������)")]
    [SerializeField] private AudioClip[] m_seClips;

    [Header("���ʐݒ�")]
    [SerializeField][Range(0f, 1f)] private float m_bgmVolume = 1f;
    [SerializeField][Range(0f, 1f)] private float m_seVolume = 1f;

    // ---- AudioSource ----
    private AudioSource m_bgmSource;
    private AudioSource m_seSource;

    // ---- �t�F�[�h���� ----
    private Coroutine m_currentFadeCoroutine;

    // ---- �v���p�e�B ----
    public float BGMVolume
    {
        get => m_bgmVolume;
        set
        {
            m_bgmVolume = Mathf.Clamp01(value);
            if (m_bgmSource != null && !IsFading)
            {
                m_bgmSource.volume = m_bgmVolume;
            }
        }
    }

    public float SEVolume
    {
        get => m_seVolume;
        set => m_seVolume = Mathf.Clamp01(value);
    }

    private bool IsFading => m_currentFadeCoroutine != null;

    // ---- ������ ----
    private void Awake()
    {
        // Singleton������
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // AudioSource���Z�b�g�A�b�v
        m_bgmSource = gameObject.AddComponent<AudioSource>();
        m_bgmSource.loop = true;
        m_bgmSource.volume = m_bgmVolume;

        m_seSource = gameObject.AddComponent<AudioSource>();
        m_seSource.loop = false;
    }

    // ---- BGM�Đ� ----
    public void PlayBGM(BGMType type, float fadeTime = 0.5f)
    {
        int index = (int)type;
        if (!IsValidIndex(index, m_bgmClips)) return;

        AudioClip nextClip = m_bgmClips[index];
        if (m_bgmSource.clip == nextClip && m_bgmSource.isPlaying) return;

        // �����̃t�F�[�h�������~
        if (m_currentFadeCoroutine != null)
        {
            StopCoroutine(m_currentFadeCoroutine);
        }

Debug.Log($"SoundManager: PlayBGM {type}");

        m_currentFadeCoroutine = StartCoroutine(ChangeBGM(nextClip, fadeTime));
    }

    private IEnumerator ChangeBGM(AudioClip nextClip, float fadeTime)
    {
        // �t�F�[�h�A�E�g
        if (m_bgmSource.isPlaying)
        {
            float startVolume = m_bgmSource.volume;
            for (float t = 0; t < fadeTime; t += Time.deltaTime)
            {
                m_bgmSource.volume = Mathf.Lerp(startVolume, 0, t / fadeTime);
                yield return null;
            }
            m_bgmSource.Stop();
        }

        // �V����BGM�Đ�
        m_bgmSource.clip = nextClip;
        m_bgmSource.volume = 0;
        m_bgmSource.Play();

        // �t�F�[�h�C��
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            m_bgmSource.volume = Mathf.Lerp(0, m_bgmVolume, t / fadeTime);
            yield return null;
        }
        m_bgmSource.volume = m_bgmVolume;

        m_currentFadeCoroutine = null;
    }

    // ---- SE�Đ� ----
    public void PlaySE(SEType type)
    {
        int index = (int)type;
        if (!IsValidIndex(index, m_seClips)) return;

        m_seSource.PlayOneShot(m_seClips[index], m_seVolume);

        Debug.Log($"SoundManager: PlaySE {type}");
    }

    // ---- BGM��~ ----
    public void StopBGM(float fadeTime = 0.5f)
    {
        if (m_currentFadeCoroutine != null)
        {
            StopCoroutine(m_currentFadeCoroutine);
        }
        m_currentFadeCoroutine = StartCoroutine(FadeOutBGM(fadeTime));
    }

    private IEnumerator FadeOutBGM(float fadeTime)
    {
        float startVolume = m_bgmSource.volume;
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            m_bgmSource.volume = Mathf.Lerp(startVolume, 0, t / fadeTime);
            yield return null;
        }
        m_bgmSource.Stop();
        m_bgmSource.volume = m_bgmVolume; // �ݒ肳�ꂽ���ʂɖ߂�

        m_currentFadeCoroutine = null;
    }

    // ---- BGM�ꎞ��~/�ĊJ ----
    public void PauseBGM()
    {
        m_bgmSource.Pause();
    }

    public void ResumeBGM()
    {
        m_bgmSource.UnPause();
    }

    // ---- ���[�e�B���e�B ----
    private bool IsValidIndex(int index, AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0)
        {
            Debug.LogError("SoundManager: AudioClip�z�񂪐ݒ肳��Ă��܂���");
            return false;
        }
        if (index < 0 || index >= clips.Length)
        {
            Debug.LogError($"SoundManager: �C���f�b�N�X���͈͊O�ł� (index={index}, length={clips.Length})");
            return false;
        }
        if (clips[index] == null)
        {
            Debug.LogError($"SoundManager: AudioClip��null�ł� (index={index})");
            return false;
        }
        return true;
    }
}
