using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class atkEnemy : MonoBehaviour
{
    // �萔
    private enum SEKind
    {
        Sweep = 0,
        SwingDown,
        SE_MAX
    }

    // �R���|�[�l���g
    [SerializeField] Transform player;
    [SerializeField] GameObject[] bodyParts_Trail; //�U���̋O�Ղ�\�������邽�߂̕ϐ�
    [SerializeField, NamedArray(typeof(SEKind))]
    AudioClip[] atkSE = new AudioClip[(int)SEKind.SE_MAX];
    AudioSource audioSource;
    Animator animator;

    [SerializeField, NamedArray(typeof(SEKind)), Range(0.0f, 1.0f)]
    float[] SEValue = new float[(int)SEKind.SE_MAX];  // SE�̉���
    int oldAttack;
    bool isAttack;

    //�U���̋O�Ղ�\������葫�̖��O
    private enum bodyPartsKind
    {
        L_HAND  = 0,    //����
        R_HAND  = 1,    //�E��
        L_LEG  = 2,     //����
        R_LEG  = 3,     //�E��
    }

    // Start is called before the first frame update
    void Start()
    {
        //audioSource = GetComponent<AudioSource>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        oldAttack = -1;
        transform.LookAt(player);
    }

    // Update is called once per frame
    void Update()
    {
        // �U���ʒu���m�F����
        if (GameManager.Second > LoadChartData.Data.Notes[GameManager.DataIdx].Time)
        {
            if (oldAttack != GameManager.DataIdx)
            {
                oldAttack = GameManager.DataIdx;

                // �U���\���f�[�^���m�F����
                if (LoadChartData.Data.Notes[GameManager.DataIdx].IsAtkPredict)
                {
                    isAttack = true;
                    // �m�[�g�𐶐�
                    NoteManager.CreateNote(LoadChartData.Data.Notes[GameManager.DataIdx].Kind, true,GameManager.DataIdx);

                    // �U���\�����U��(�S��)
                    // �G�̍U�����[�V�����J�n����U���^�C�~���O�܂łQ��
                    // �P�����̕b���A�\���U������x�点�ă��[�V�������J�n����
                    var waitAtkTime = (60.0f / LoadChartData.Data.Bpm);
                    Invoke("PlayAnim", waitAtkTime);
                }
                else
                {
                    isAttack = false;
                }
            }
        }

        if (!isAttack)
            transform.LookAt(player);
    }

    void FixedUpdate()
    {
        
    }

    void ActivePartsTrail(bodyPartsKind kind)
    {
        bodyParts_Trail[(int)kind].SetActive(true);
    }

    public void DeactivateAllPartsTrail()
    {
        foreach (var part in bodyParts_Trail)
        {
            part.SetActive(false);
        }
    }

    void PlaySE(AudioClip[] se, float[] volume, int index)
    {
        audioSource.PlayOneShot(se[index]);
        audioSource.volume = volume[index];
    }

    void PlayAnim()
    {
        switch (LoadChartData.Data.Notes[GameManager.DataIdx].Kind)
        {
            case LoadChartData.NoteKind.E_UP_L:
                DeactivateAllPartsTrail(); //�O��\�������O�Ղ��\���ɖ߂��Ƃ�

                animator.SetTrigger("B_UP");
                // SE�Đ�
                //PlaySE(atkSE, SEValue, (int)SEKind.SwingDown);
                ActivePartsTrail(bodyPartsKind.L_HAND);
                ActivePartsTrail(bodyPartsKind.R_HAND);
                break;

            case LoadChartData.NoteKind.E_UP_S:
                DeactivateAllPartsTrail();

                animator.SetTrigger("S_UP");
                // SE�Đ�
                //PlaySE(atkSE, SEValue, (int)SEKind.SwingDown);
                ActivePartsTrail(bodyPartsKind.R_HAND);
                break;

            case LoadChartData.NoteKind.E_DOWN_L:
                DeactivateAllPartsTrail();

                animator.SetTrigger("B_Down");
                // SE�Đ�
                //PlaySE(atkSE, SEValue, (int)SEKind.SwingDown);
                ActivePartsTrail(bodyPartsKind.L_HAND);
                ActivePartsTrail(bodyPartsKind.R_HAND);
                break;

            case LoadChartData.NoteKind.E_DOWN_S:
                DeactivateAllPartsTrail();

                animator.SetTrigger("S_Down");
                // SE�Đ�
                //PlaySE(atkSE, SEValue, (int)SEKind.SwingDown);
                ActivePartsTrail(bodyPartsKind.L_HAND);
                break;

            case LoadChartData.NoteKind.E_RIGHT_L:
                DeactivateAllPartsTrail();

                animator.SetTrigger("B_Right");
                // SE�Đ�
                //PlaySE(atkSE, SEValue, (int)SEKind.Sweep);
                ActivePartsTrail(bodyPartsKind.L_HAND);
                ActivePartsTrail(bodyPartsKind.R_HAND);
                break;

            case LoadChartData.NoteKind.E_RIGHT_S:
                DeactivateAllPartsTrail();

                animator.SetTrigger("S_Right");
                // SE�Đ�
                //PlaySE(atkSE, SEValue, (int)SEKind.Sweep);
                ActivePartsTrail(bodyPartsKind.L_HAND);
                ActivePartsTrail(bodyPartsKind.L_LEG);
                break;

            case LoadChartData.NoteKind.E_LEFT_L:
                DeactivateAllPartsTrail();

                animator.SetTrigger("B_Left");
                // SE�Đ�
                //PlaySE(atkSE, SEValue, (int)SEKind.Sweep);
                ActivePartsTrail(bodyPartsKind.L_HAND);
                ActivePartsTrail(bodyPartsKind.R_HAND);
                break;

            case LoadChartData.NoteKind.E_LEFT_S:
                DeactivateAllPartsTrail();

                animator.SetTrigger("S_Left");
                // SE�Đ�
                //PlaySE(atkSE, SEValue, (int)SEKind.Sweep);
                ActivePartsTrail(bodyPartsKind.L_HAND);
                ActivePartsTrail(bodyPartsKind.L_LEG);
                break;
        }
    }
}
