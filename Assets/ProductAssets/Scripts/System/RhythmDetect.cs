using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RhythmDetect : MonoBehaviour
{
    [SerializeField] float StylishRange;
    [SerializeField] float GoodRange;
    [SerializeField] AudioClip audioClip;

    

    /// <summary> �X�^�C���b�V������̔���b�� </summary>
    public static float STYLISH_RANGE { get; set; }

    /// <summary> ���ʉ���̔���b��</summary>
    public static float GOOD_RANGE { get; set; }

    public static float DODGE_RANGE { get; set; }

    static bool isInput = false;


    /// <summary>
    /// ���蒆���ǂ���
    /// </summary>
    public static bool IsInput
    {
        get { return isInput; }
        private set { isInput = value; }
    }

    /// <summary>
    /// ���݂̍Đ����Ԃ��m�[�c���肩�H
    /// </summary>
    public static bool IsJudging {  get; private set; }

    /// <summary>
    /// ���肳�ꂽ�^�C�~���O�̎��
    /// </summary>
    public enum Dodge
    {
        none,
        miss,
        good,
        stylish
    }

    static Dodge result = Dodge.none;

    static public Dodge Result
    {
        get { return result; }
        private set {; }
    }
    // Start is called before the first frame update
    void Start()
    {
        STYLISH_RANGE = StylishRange; 
        GOOD_RANGE = GoodRange;
    }

    // Update is called once per frame
    void Update()
    {
        if (LoadChartData.Data.Notes[GameManager.DataIdx].IsAtkPredict)
        {
            isInput = false;
        }
        else
        {
            result = Dodge.none;
            // �m�[�c�̔��蒆�����擾
            var time = LoadChartData.Data.Notes[GameManager.DataIdx].Time;
            IsJudging = (time - GoodRange < GameManager.Second && time + GoodRange > GameManager.Second);
        }

    }

    /// <summary>
    /// ������Ƃ�
    /// </summary>
    /// <param name="justTiming"></param>
    /// <param name="kind">������</param>
    /// <returns></returns>
    public static void DodgeDetect(in float justTiming, in LoadChartData.NoteKind kind)
    {
        if (isInput) return;

        // �������L�[���`�F�b�N
        if (MyInput.GetKeyDown(kind))
        {
            isInput = true;
            CheckRanged(justTiming, GameManager.Second);
        }
        else
        {
            // ����Ƃ͈Ⴄ�L�[��������Ă��Ȃ����`�F�b�N
            // ����̎�ނ������傩�ŃL�[���͂������債�����肵�Ȃ�
            for (int i = (int)kind < 4 ? 0 : 4;
                i < ((int)kind > 4 ? 4 : 8);
                i++)
            {
                if (MyInput.GetKeyDown((LoadChartData.NoteKind)i))
                {
                    isInput = true;
                    result = Dodge.miss;
                    Debug.Log("���̓L�[���Ⴂ�܂�");
                    return;
                }
            }
        }

        // �w�莞�ԓ��ɓ���̓��͂��Ȃ�
        if (!isInput && GameManager.Second > justTiming + GOOD_RANGE)
        {
            isInput = true;
            Debug.Log("no key");
            result = Dodge.miss;
        }
    }


    //--- �T�u���[�`��

    /// <summary>
    /// ���背���W�œ��͂��ꂽ�����ʂ��i�[���܂�
    /// </summary>
    /// <param name="justTiming">���肪�s���鎞��</param>
    /// <param name="currentTime">���݂̎���</param>
    static void CheckRanged(in float justTiming, in float currentTime)
    {
        // ���莞�ԂƂ̌덷�����߂�
        float timeDifference = Mathf.Abs(justTiming - currentTime);

        Debug.Log(timeDifference);

        // �덷����ǂ̃����W�ɂ��邩���ׂ�
        if (timeDifference <= STYLISH_RANGE)
            result = Dodge.stylish;

        else if (timeDifference <= GOOD_RANGE)
            result = Dodge.good;

        else
        {
            Debug.Log("�x��");
            result = Dodge.miss;
        }
    }

    /// <summary>
    /// �������L�[�������ꂽ������
    /// </summary>
    /// <param name="key">�������L�[</param>
    /// <param name="sucsess">�������̉��</param>
    /// <returns>���茋��</returns>
    static Dodge KeyChecker(Dodge sucsess, KeyCode key1, KeyCode key2)
    {
        //--- ����L�[���P��
        if(key2 == KeyCode.None)
        {
            // �������L�[�������ꂽ�H
            if (Input.GetKeyDown(key1))
            {
                isInput = true;
                return sucsess;
            
            }
            // �Ԉ�����L�[�������ꂽ�H
            else if (Input.anyKeyDown)
            {
                isInput = true;
                return Dodge.miss;
            }
            // �L�[��������Ȃ�����
            return Dodge.none;
        }
        else
        {
            // ����L�[���Q��
            if (Input.GetKey(key1) && Input.GetKeyDown(key2) ||
                Input.GetKeyDown(key1) && Input.GetKey(key2))
            {
                isInput = true;
                return sucsess;
            }
            // �Ԉ�����L�[�������ꂽ�H
            else if (!Input.GetKey(key1) && Input.anyKeyDown)
            {
                isInput = true;
                Debug.Log("miss key");
                return Dodge.miss;
            }
            // �L�[��������Ȃ�����
            return Dodge.none;
        }
    }

    /// <summary>
    /// �����Ŏw�肵���L�[�̂ǂꂩ�P�ł�
    /// ���͂���Ă��邩���肷��
    /// </summary>
    /// <param name="keys">[params] ���͔�����s���L�[</param>
    /// <returns>���͂����ꂽ��</returns>
    static bool IsKeyPressed(params KeyCode[] keys)
    {
        foreach (KeyCode keyCode in keys)
        {
            if (Input.GetKey(keyCode))
            {
                return true;
            }
        }
        return false;
    }
}
