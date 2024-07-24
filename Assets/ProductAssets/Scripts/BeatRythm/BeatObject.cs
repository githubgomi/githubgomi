using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class BeatController : MonoBehaviour
{
    [SerializeField] float bpm = 120f;
    [SerializeField] Vector3 maxScale = new Vector3(1.5f, 1.5f, 1.5f);
    [SerializeField] Vector3 minScale = new Vector3(1f, 1f, 1f);
    [SerializeField] float scaleDuration = 0.1f;
    [SerializeField] float rotationAngle = 90f;
    [SerializeField] bool clockwise = true;
    [SerializeField] Vector3 rotationAxis = Vector3.forward;
    [SerializeField] AudioSource audioSource;
    [SerializeField] float beatDetectionThreshold = 0.5f;

    private float[] spectrumData = new float[1024];
    private float beatInterval;
    private float lastBeatTime;
    private List<Transform> childObjects = new List<Transform>();

    void Start()
    {
        beatInterval = 60f / bpm;
        lastBeatTime = -beatInterval;

        // 子オブジェクトをリストに追加
        foreach (Transform child in transform)
        {
            childObjects.Add(child);
        }
    }

    void Update()
    {
        if (audioSource.isPlaying)
        {
            DetectBeat();
        }
    }

    void DetectBeat()
    {
        audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.BlackmanHarris);

        float maxSpectrum = 0f;
        for (int i = 0; i < spectrumData.Length; i++)
        {
            if (spectrumData[i] > maxSpectrum)
            {
                maxSpectrum = spectrumData[i];
            }
        }

        if (maxSpectrum > beatDetectionThreshold && Time.time > lastBeatTime + beatInterval)
        {
            OnBeat();
            lastBeatTime = Time.time;
        }
    }

    void OnBeat()
    {
        foreach (Transform child in childObjects)
        {
            ScaleObject(child);
            RotateObject(child);
        }
    }

    void ScaleObject(Transform obj)
    {
        obj.DOScale(maxScale, scaleDuration / 2)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                obj.DOScale(minScale, scaleDuration / 2)
                    .SetEase(Ease.InQuad);
            });
    }

    void RotateObject(Transform obj)
    {
        float finalRotationAngle = clockwise ? rotationAngle : -rotationAngle;
        Vector3 rotationVector = rotationAxis.normalized * finalRotationAngle;

        obj.DORotate(obj.eulerAngles + rotationVector, scaleDuration)
            .SetEase(Ease.Linear);
    }
}