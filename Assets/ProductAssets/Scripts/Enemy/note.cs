using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Note : MonoBehaviour
{
    [SerializeField] float moveDistancce = 5.0f;// �ړ�����
    float moveTime = RhythmDetect.GOOD_RANGE;     // �ړ��ɂ����鎞��

    private Vector3 startPos;   // �����������W
    private Vector3 movePos;    // �ړ�����
    private float speed;        // �ړ��X�s�[�h

    bool isEnable = true;       // �g�p����Ă��邩

    public bool IsEnable{
        get { return isEnable; }
        private set {; }
    }

    // Start is called before the first frame update
    void Start()
    {
        speed = moveDistancce / (moveTime * 60.0f);
    }

    // Update is called once per frame
    void Update()
    {
        // �I�u�W�F�N�g�̍폜
        if (moveTime * 60.0f < 0.0f)
            isEnable = false;   // �}�l�[�W���[�ŃC���X�^���X�̍폜�����������ߍ폜�t���O�ŊǗ�(�v����)
    }

    private void FixedUpdate()
    {
        // ���W�X�V
        startPos = startPos + movePos * speed;
        transform.SetPositionAndRotation(startPos,Quaternion.identity);

        moveTime -= Time.deltaTime;
    }
    
    /// <summary>
    /// �m�[�c�̍��W��ݒ�
    /// </summary>
    /// <param name="type"></param>
    /// <param name="pos"></param>
    public void SetNote(LoadChartData.NoteKind type, Transform pos)
    {
        // startPos = ��{���W +- �����x�N�g�� * (�ړ����� * 0.5f)
        switch(type)
        {
            case LoadChartData.NoteKind.E_UP_S:
            case LoadChartData.NoteKind.E_UP_L: // L�͌�X�ύX����
                startPos = pos.position + pos.up * (moveDistancce * 0.5f);
                movePos = -pos.up;
                break;
            case LoadChartData.NoteKind.E_DOWN_S:
            case LoadChartData.NoteKind.E_DOWN_L:
                startPos = pos.position + -pos.up * (moveDistancce * 0.5f);
                movePos = pos.up;
                break;
            case LoadChartData.NoteKind.E_LEFT_S:
            case LoadChartData.NoteKind.E_LEFT_L:
                startPos = pos.position + -pos.right * (moveDistancce * 0.5f);
                movePos = pos.right;
                break;
            case LoadChartData.NoteKind.E_RIGHT_S:
            case LoadChartData.NoteKind.E_RIGHT_L:
                startPos = pos.position + pos.right * (moveDistancce * 0.5f);
                movePos = -pos.right;
                break;
            default:
                break;
        }       
    }
}
