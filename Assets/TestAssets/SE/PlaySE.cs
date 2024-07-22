using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySE : MonoBehaviour
{
    //--- コンポーネント
    AudioSource audioSource;

    //--- データ
    // SE
    [SerializeField]
    AudioClip[] SE;
    // 音量
    [SerializeField, Range(0.0f, 1.0f)]
    float[] SEVolume;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Play(int index)
    {
        audioSource.PlayOneShot(SE[index]);
        audioSource.volume = SEVolume[index];
    }
}
