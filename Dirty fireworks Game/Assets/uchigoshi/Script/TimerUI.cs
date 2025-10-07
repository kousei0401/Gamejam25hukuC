using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TimerUI : MonoBehaviour
{
    [Header("QÆ")]
    public GameTimer gameTimer;
    public Slider timeSlider;
    public Text timeText;

    public void UpdateUI(float currentTime, float maxTime)
    {
        timeSlider.value = currentTime / maxTime;
        timeText.text = $"c‚èŠÔ: {currentTime:F1}•b";
    }
}
