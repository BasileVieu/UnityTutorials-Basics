using System;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] private Transform hoursPivot;
    [SerializeField] private Transform minutesPivot;
    [SerializeField] private Transform secondsPivot;

    private const float hoursToDegrees = -30.0f;
    private const float minutesToDegrees = -6.0f;
    private const float secondsToDegrees = -6.0f;

    void Update()
    {
        TimeSpan time = DateTime.Now.TimeOfDay;

        hoursPivot.localRotation = Quaternion.Euler(0.0f, 0.0f, hoursToDegrees * (float)time.TotalHours);
        minutesPivot.localRotation = Quaternion.Euler(0.0f, 0.0f, minutesToDegrees * (float)time.TotalMinutes);
        secondsPivot.localRotation = Quaternion.Euler(0.0f, 0.0f, secondsToDegrees * (float)time.TotalSeconds);
    }
}