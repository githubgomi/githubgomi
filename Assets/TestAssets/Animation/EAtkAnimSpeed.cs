using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EAtkAnimSpeed : MonoBehaviour
{
    /// <summary>
    /// �A�j���[�V�������x
    /// </summary>
    public static float Speed { get; private set; } = 1.0f;

    /// <summary>
    /// �A�j���[�V�����R���|�[�l���g
    /// </summary>
    //[SerializeField] Animator serialize_animator;
    static Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>(); // static�ϐ��Ɏ擾����animator���Z�b�g
    }

    // Update is called once per frame
    void Update()
    {
        Speed = LoadChartData.Data.Bpm / 120; // �P�r�[�g������̕b��
        animator.SetFloat("Speed", Speed);
    }

    public static void SetSpeedToBPM(in float bpm)
    {
        Speed = bpm / 120; // �P�r�[�g������̕b��
        animator.SetFloat("Speed", Speed);
    }
}
