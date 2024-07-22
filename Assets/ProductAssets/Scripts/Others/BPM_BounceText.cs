using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BPM_BounceText : MonoBehaviour
{
    [SerializeField] private float bpm = 120f;
    [SerializeField] private float scaleFactor = 1.2f;
    [SerializeField] private float scaleSpeed = 0.5f;
    
    private RectTransform rectTransform;
    private Vector3 originalScale;
    private float beatInterval;
    private float timer;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;
        beatInterval = 60f / bpm;
        timer = 0f;

        StartScaling();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= beatInterval)
        {
            timer -= beatInterval;
            StartScaling();
        }
    }

    private void StartScaling()
    {
        float scaleDuration = beatInterval * scaleSpeed;
        float waitDuration = Mathf.Max(0, beatInterval - scaleDuration * 2);

        Sequence scaleSequence = DOTween.Sequence();

        // 拡大
        scaleSequence.Append(rectTransform.DOScale(originalScale * scaleFactor, scaleDuration / 2));
        
        // 縮小
        scaleSequence.Append(rectTransform.DOScale(originalScale, scaleDuration / 2));

        // 待機（必要な場合）
        if (waitDuration > 0)
        {
            scaleSequence.AppendInterval(waitDuration);
        }

        scaleSequence.SetEase(Ease.InOutQuad);
    }
}