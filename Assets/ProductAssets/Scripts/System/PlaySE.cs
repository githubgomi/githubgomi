using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySE : MonoBehaviour
{
    //--- �R���|�[�l���g
    AudioSource audioSource;

    //--- �f�[�^
    // SE
    [SerializeField]
    AudioClip[] SE;
    // ����
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
