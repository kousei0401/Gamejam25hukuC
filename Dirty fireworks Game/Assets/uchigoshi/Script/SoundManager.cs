using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ゲーム全体のサウンド（BGM・SE）を統一管理するクラス
/// 他のスクリプトからは「SoundManager.Instance」でアクセス可能
/// 例 ：SoundManager.Instance.PlayBGM(SoundManager.BGMType.BattleBGM);
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

    // ---- Inspector設定 ----
    [Header("BGM Clips (Enumの順に設定してください)")]
    [SerializeField] private AudioClip[] m_bgmClips;

    [Header("SE Clips (Enumの順に設定してください)")]
    [SerializeField] private AudioClip[] m_seClips;

    [Header("音量設定")]
    [SerializeField][Range(0f, 1f)] private float m_bgmVolume = 1f;
    [SerializeField][Range(0f, 1f)] private float m_seVolume = 1f;

    // ---- AudioSource ----
    private AudioSource m_bgmSource;
    private AudioSource m_seSource;

    // ---- フェード制御 ----
    private Coroutine m_currentFadeCoroutine;

    // ---- プロパティ ----
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

    // ---- 初期化 ----
    private void Awake()
    {
        // Singleton初期化
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // AudioSourceをセットアップ
        m_bgmSource = gameObject.AddComponent<AudioSource>();
        m_bgmSource.loop = true;
        m_bgmSource.volume = m_bgmVolume;

        m_seSource = gameObject.AddComponent<AudioSource>();
        m_seSource.loop = false;
    }

    // ---- BGM再生 ----
    public void PlayBGM(BGMType type, float fadeTime = 0.5f)
    {
        int index = (int)type;
        if (!IsValidIndex(index, m_bgmClips)) return;

        AudioClip nextClip = m_bgmClips[index];
        if (m_bgmSource.clip == nextClip && m_bgmSource.isPlaying) return;

        // 既存のフェード処理を停止
        if (m_currentFadeCoroutine != null)
        {
            StopCoroutine(m_currentFadeCoroutine);
        }

Debug.Log($"SoundManager: PlayBGM {type}");

        m_currentFadeCoroutine = StartCoroutine(ChangeBGM(nextClip, fadeTime));
    }

    private IEnumerator ChangeBGM(AudioClip nextClip, float fadeTime)
    {
        // フェードアウト
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

        // 新しいBGM再生
        m_bgmSource.clip = nextClip;
        m_bgmSource.volume = 0;
        m_bgmSource.Play();

        // フェードイン
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            m_bgmSource.volume = Mathf.Lerp(0, m_bgmVolume, t / fadeTime);
            yield return null;
        }
        m_bgmSource.volume = m_bgmVolume;

        m_currentFadeCoroutine = null;
    }

    // ---- SE再生 ----
    public void PlaySE(SEType type)
    {
        int index = (int)type;
        if (!IsValidIndex(index, m_seClips)) return;

        m_seSource.PlayOneShot(m_seClips[index], m_seVolume);

        Debug.Log($"SoundManager: PlaySE {type}");
    }

    // ---- BGM停止 ----
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
        m_bgmSource.volume = m_bgmVolume; // 設定された音量に戻す

        m_currentFadeCoroutine = null;
    }

    // ---- BGM一時停止/再開 ----
    public void PauseBGM()
    {
        m_bgmSource.Pause();
    }

    public void ResumeBGM()
    {
        m_bgmSource.UnPause();
    }

    // ---- ユーティリティ ----
    private bool IsValidIndex(int index, AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0)
        {
            Debug.LogError("SoundManager: AudioClip配列が設定されていません");
            return false;
        }
        if (index < 0 || index >= clips.Length)
        {
            Debug.LogError($"SoundManager: インデックスが範囲外です (index={index}, length={clips.Length})");
            return false;
        }
        if (clips[index] == null)
        {
            Debug.LogError($"SoundManager: AudioClipがnullです (index={index})");
            return false;
        }
        return true;
    }
}
