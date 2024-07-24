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
    Animator animator;

    int oldAttack;
    bool isAttack;

    //�U���̋O�Ղ�\������葫�̖��O
    private enum bodyPartsKind
    {
        L_HAND = 0,    //����
        R_HAND,         //�E��
        L_LEG,          //����
        R_LEG,          //�E��
    }

    // Start is called before the first frame update
    void Start()
    {
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

    void PlayAnim()
    {
        switch (LoadChartData.Data.Notes[GameManager.DataIdx].Kind)
        {
            case LoadChartData.NoteKind.E_UP_L:
                DeactivateAllPartsTrail(); //�O��\�������O�Ղ��\���ɖ߂��Ƃ�

                animator.SetTrigger("B_UP");
                ActivePartsTrail(bodyPartsKind.L_HAND);
                ActivePartsTrail(bodyPartsKind.R_HAND);
                break;

            case LoadChartData.NoteKind.E_UP_S:
                DeactivateAllPartsTrail();

                animator.SetTrigger("S_UP");
                ActivePartsTrail(bodyPartsKind.R_HAND);
                break;

            case LoadChartData.NoteKind.E_DOWN_L:
                DeactivateAllPartsTrail();

                animator.SetTrigger("B_Down");
                ActivePartsTrail(bodyPartsKind.L_HAND);
                ActivePartsTrail(bodyPartsKind.R_HAND);
                break;

            case LoadChartData.NoteKind.E_DOWN_S:
                DeactivateAllPartsTrail();

                animator.SetTrigger("S_Down");
                ActivePartsTrail(bodyPartsKind.L_HAND);
                break;

            case LoadChartData.NoteKind.E_RIGHT_L:
                DeactivateAllPartsTrail();

                animator.SetTrigger("B_Right");
                ActivePartsTrail(bodyPartsKind.L_HAND);
                ActivePartsTrail(bodyPartsKind.R_HAND);
                break;

            case LoadChartData.NoteKind.E_RIGHT_S:
                DeactivateAllPartsTrail();

                animator.SetTrigger("S_Right");
                ActivePartsTrail(bodyPartsKind.L_HAND);
                ActivePartsTrail(bodyPartsKind.L_LEG);
                break;

            case LoadChartData.NoteKind.E_LEFT_L:
                DeactivateAllPartsTrail();

                animator.SetTrigger("B_Left");
                ActivePartsTrail(bodyPartsKind.L_HAND);
                ActivePartsTrail(bodyPartsKind.R_HAND);
                break;

            case LoadChartData.NoteKind.E_LEFT_S:
                DeactivateAllPartsTrail();

                animator.SetTrigger("S_Left");
                ActivePartsTrail(bodyPartsKind.L_HAND);
                ActivePartsTrail(bodyPartsKind.L_LEG);
                break;
        }
    }
}
