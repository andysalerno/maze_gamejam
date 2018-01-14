using System;
using UnityEngine;
using UnityEngine.UI;

public class CountdownDisplay : MonoBehaviour
{
    public float timeOnclockMs;
    private float remainingTimeMs;

    private Text countdownText;

    private PlayerInteract playerInteract;

    // has the timer already triggered for < 0 time?
    private bool timerTriggered = false;

    void Start()
    {
        this.ResetTimer();
        this.countdownText = this.GetComponentInChildren<Text>();
        this.playerInteract = this.GetComponentInChildren<PlayerInteract>();
    }

    void Update()
    {
        this.remainingTimeMs -= Time.deltaTime * 1000;
        var minuteSecond = new TimeSpan(0, 0, 0, 0, (int)this.remainingTimeMs);
        this.countdownText.text = string.Format("{0}:{1:D2}", minuteSecond.Minutes, minuteSecond.Seconds);

        if (this.remainingTimeMs <= 0 && !this.timerTriggered)
        {
            this.timerTriggered = true;
            this.TimerUpBehavior();
        }
    }

    public void ResetTimer(float? timeOnClockMs = null)
    {
        this.remainingTimeMs = timeOnClockMs ?? this.timeOnclockMs;
        this.remainingTimeMs = this.timeOnclockMs;
        this.timerTriggered = false;
    }

    private void TimerUpBehavior()
    {
        this.playerInteract.TimerUpBehavior();
    }
}
