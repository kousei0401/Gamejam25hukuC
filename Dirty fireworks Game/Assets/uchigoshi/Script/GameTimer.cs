using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class GameTimer : MonoBehaviour
{
    [Header("タイマー設定")]
    public float startTime = 45f;  // 初期時間
    private float currentTime;
    public bool isRunning = true;

    [Header("イベント設定")]
    public UnityEvent onGameOver; // 残り時間が0になった時に呼ばれる


    [SerializeField] private TimerUI timerUI;

    private void Start()
    {
        StartTimer(startTime);
        SoundManager.Instance.PlayBGM(SoundManager.BGMType.TitleBGM);
    }

    private void Update()
    {
        if (!isRunning) return;

        currentTime -= Time.deltaTime;
        timerUI?.UpdateUI(currentTime, startTime);

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            isRunning = false;
            onGameOver?.Invoke();
            SoundManager.Instance.StopBGM();

        }
    }

    public void StartTimer(float duration)
    {
        currentTime = duration;
        isRunning = true;
        timerUI?.UpdateUI(currentTime, startTime);
    }

    public void AddTime(float value)
    {
        currentTime = Mathf.Min(currentTime + value, startTime);
        timerUI?.UpdateUI(currentTime, startTime);
    }
    public void ReduceTime(float value)
    {
        currentTime = Mathf.Min(currentTime - value, startTime);
        timerUI?.UpdateUI(currentTime, startTime);

    }
}

