using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class LoadChartData : MonoBehaviour
{
    /// <summary>
    /// �m�[�c�̎��
    /// </summary>
    public enum NoteKind
    {
        // ��
        E_UP_S,
        E_DOWN_S,
        E_LEFT_S,
        E_RIGHT_S,
        // ��
        E_UP_L,
        E_DOWN_L,
        E_LEFT_L,
        E_RIGHT_L,
    }

    /// <summary>
    /// ���ʃf�[�^
    /// </summary>
    public struct ChartData
    {
        /// <summary>�ȃ^�C�g��</summary>
        public string Title { get; set; }
        /// <summary>�P���Ԃ̔���</summary>
        public float Bpm { get; set; }

        /// <summary>�Ȃ̃X�^�[�g(s)</summary>
        public float Start { get; set; }

        /// <summary>�Ȃ̏I���(s)</summary>
        public float End { get; set; }
        /// <summary>�m�[�c�i�[���X�g</summary>
        public List<Note> Notes { get; set; }
    }

    /// <summary>
    /// �m�[�c�f�[�^
    /// </summary>
    public struct Note
    {
        /// <summary>���</summary>
        public NoteKind Kind { get; set; }
        /// <summary>����</summary>
        public float Time { get; set; }
        /// <summary>�X�^�C���b�V���J�������L���H</summary>
        public bool IsEnableCamera { get; set; }
        /// <summary>�m�[�c���U���\���H</summary>
        public bool IsAtkPredict { get; set; }
    }

    //--- �ϐ��錾 ---
    /// <summary> ���ʃf�[�^ </summary>
    public static ChartData Data = new ChartData();// �Q�[������������ǂݍ��܂Ȃ��̂�static�ŕێ�


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    struct ReadNote
    {
        public int Kind { get; set; }

        public double Time { get; set; }

        public bool IsEnableCamera { get; set; }

        public bool IsAtkPredict { get; set; }
    }

    /// <summary>
    /// ���ʃf�[�^�ǂݍ���
    /// </summary>
    public static void Load(in int fileNo)
    {
        // ������
        Data = new ChartData();
        Data.Notes = new List<Note>();

        var text = File.ReadAllText(Application.dataPath + "/" + "ProductAssets/Data/Stage" + fileNo.ToString() + ".json");

        Data = JsonConvert.DeserializeObject<ChartData>(text);
    }

    //--- �T�u���[�`��
 }


