using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_StageSelect : MonoBehaviour
{
    // Start is called before the first frame update


    public float Speed;          //�ړ��X�s�[�h
                                 //
    private int Index = 0;      //�I���X�e�[�W
    int MaxStage = 0;            //�ő�X�e�[�W��
   

    //�e�I�u�W�F�N�g
    GameObject StageObject;

    //�q�I�u�W�F�N�g
    Transform[] StagePos;

    Vector3 TargetPos;   //�X�e�[�W���̏ꏊ


    void Start()
    {
        //�X�e�[�W�̐e�I�u�W�F�N�g���擾
        StageObject = GameObject.Find("StageObject");

        //�q�����̔z����m��
        StagePos = new Transform[StageObject.transform.childCount];

        //�q���̃g�����X�t�H�[���������Ă���
        for(int i =0;i<StageObject.transform.childCount;i++)
        {
            StagePos[i] = StageObject.transform.GetChild(i);
        }

        //�ő�X�e�[�W��
        MaxStage = StageObject.transform.childCount;

       

       TargetPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {       
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (Index < MaxStage -1)
            {
                Index++;
                //�w�肵���X�e�[�W�̍��W�������Ă���
                TargetPos.x = StagePos[Index].position.x;              
            } 
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
           if(Index > 0)
            {
                Index--;
                //�w�肵���X�e�[�W�̍��W�������Ă���
                TargetPos.x = StagePos[Index].position.x;
            }

        }

        //�ڕW�̂x��Z���W�͂���Ȃ��̂Ńv���C���[�̍��W�ŏ㏑��
        TargetPos.y = transform.position.y;
        TargetPos.z = transform.position.z;

        //�ڕW�̍��W�Ɍ�����
        transform.position = Vector3.MoveTowards(transform.position, TargetPos, Speed);

        if (Input.GetKeyDown(KeyCode.Return))
        {
            S_SceneChange.SetNextScene(Index);
        }

    }


  



}




