using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineTimeScaleController : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public float startTime = 5.0f;
    public float endTime = 10.0f;
    public float newTimeScale = 0.5f;
    private bool isTimeScaleChanged = false;

    void Update()
    {
        double currentTime = playableDirector.time;

        if (currentTime >= startTime && currentTime <= endTime && !isTimeScaleChanged)
        {
            Time.timeScale = newTimeScale;
            isTimeScaleChanged = true;
        }
        else if ((currentTime < startTime || currentTime > endTime) && isTimeScaleChanged)
        {
            Time.timeScale = 1.0f;
            isTimeScaleChanged = false;
        }
    }
}
