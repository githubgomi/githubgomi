using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class MyInput : MonoBehaviour
{
    /// <summary>
    /// �L�[���
    /// </summary>
    public enum Key
    {
        Up,
        Down,
        Left,
        Right,
        BigOption,

        Max
    }

    /// <summary>
    /// ���l�̓��͂��󂯕t����܂ł̃f�b�h�]�[��
    /// </summary>
    [SerializeField] float deadZone = 0.7f;

    //--- private
    private static bool[] oldInput = new bool[(int)Key.Max];
    private static bool[] curtInput = new bool[(int)Key.Max];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // �O��̓��͂�ێ�
        for(int i = 0; i < (int)Key.Max; ++i) { oldInput[i] = curtInput[i]; }

        // ������
        for(int i = 0; i < (int)Key.Max; ++i) { curtInput[i] = false; }
        
        // ����
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        // �X�e�B�b�N + �\���L�[ + �\���{�^��
        if (vertical > deadZone)        { curtInput[(int)Key.Up] = true; }
        else if (vertical < -deadZone)  { curtInput[(int)Key.Down] = true; }
        else if (horizontal < -deadZone){ curtInput[(int)Key.Left] = true; }
        else if (horizontal > deadZone) { curtInput[(int)Key.Right] = true; }

        // A�{�^��(xbox) + ���V�t�g
        if (Input.GetKey("joystick button 0") || Input.GetKey(KeyCode.LeftShift))
        //if (Input.GetKey("BigOption"))
        {
            curtInput[(int)Key.BigOption] = true;
        }
    }

    /// <summary>
    /// ���͂���Ă��邩����
    /// </summary>
    /// <param name="kind">���</param>
    /// <returns>���͂���Ă���H</returns>
    public static bool GetKey(in Key kind)
    {
        return curtInput[(int)kind];
    }

    /// <summary>
    /// �����ꂽ�u�Ԃ��𔻒�
    /// </summary>
    /// <param name="kind">���</param>
    /// <returns>�����ꂽ�u�ԁH</returns>
    public static bool GetKeyDown(in Key kind)
    {
        return (oldInput[(int)kind] ^ curtInput[(int)kind]) & curtInput[(int)kind];
    }

    /// <summary>
    /// ���ꂽ�u�Ԃ��𔻒�
    /// </summary>
    /// <param name="kind">���</param>
    /// <returns>���ꂽ�u�ԁH</returns>
    public static bool GetKeyRelease(in Key kind)
    {
        return (oldInput[(int)kind] ^ curtInput[(int)kind]) & oldInput[(int)kind];
    }


    public static bool GetKeyDown(in LoadChartData.NoteKind kind)
    {
        switch (kind)
        {
            // �����
            case LoadChartData.NoteKind.E_UP_S:
            case LoadChartData.NoteKind.E_DOWN_S:
            case LoadChartData.NoteKind.E_LEFT_S:
            case LoadChartData.NoteKind.E_RIGHT_S:
                return IsKeyDownSmall((Key)kind);

            // ����
            case LoadChartData.NoteKind.E_UP_L:
            case LoadChartData.NoteKind.E_DOWN_L:
            case LoadChartData.NoteKind.E_LEFT_L:
            case LoadChartData.NoteKind.E_RIGHT_L:
                return IsKeyDownBig((Key)((int)kind - 4));

            default: return false;
        }
    }

    //--- �T�u���[�`��
    static bool IsKeyDownBig(in Key key)
    {
        // �ǂ��炩�Е��̃L�[��������Ă���󋵂���
        // �����̃L�[�����͂��ꂽ�t���[���𔻒f
        bool old = oldInput[(int)Key.BigOption] & oldInput[(int)key];
        bool curt = curtInput[(int)Key.BigOption] & curtInput[(int)key];
    
        return (old ^ curt) & curt;
    }

    static bool IsKeyDownSmall(in Key key)
    {
        return (GetKeyDown(key) ^ curtInput[(int)Key.BigOption]) & GetKeyDown(key);
    }
}
