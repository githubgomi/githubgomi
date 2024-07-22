using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BPM_BounceSky : MonoBehaviour
{
    [SerializeField] private float bpm = 120f;
    [SerializeField] private float minExposure = 0f;
    [SerializeField] private float maxExposure = 1f;
    [SerializeField] private float changeSpeed = 0.5f;
    [SerializeField] private Material targetMaterial;
    
    private float beatInterval;
    private float timer;
    private static readonly int ExposureProperty = Shader.PropertyToID("_Exposure");

    private void Start()
    {
        if (targetMaterial == null)
        {
            Debug.LogError("Target material is not assigned!");
            enabled = false;
            return;
        }

        beatInterval = 60f / bpm;
        timer = 0f;

        StartExposureChange();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= beatInterval)
        {
            timer -= beatInterval;
            StartExposureChange();
        }
    }

    private void StartExposureChange()
    {
        float changeDuration = beatInterval * changeSpeed;
        float waitDuration = Mathf.Max(0, beatInterval - changeDuration * 2);

        Sequence exposureSequence = DOTween.Sequence();

        // 最大値に変更
        exposureSequence.Append(DOTween.To(() => targetMaterial.GetFloat(ExposureProperty),
            value => targetMaterial.SetFloat(ExposureProperty, value),
            maxExposure, changeDuration / 2));
        
        // 最小値に変更
        exposureSequence.Append(DOTween.To(() => targetMaterial.GetFloat(ExposureProperty),
            value => targetMaterial.SetFloat(ExposureProperty, value),
            minExposure, changeDuration / 2));

        // 待機（必要な場合）
        if (waitDuration > 0)
        {
            exposureSequence.AppendInterval(waitDuration);
        }

        exposureSequence.SetEase(Ease.InOutQuad);
    }
}