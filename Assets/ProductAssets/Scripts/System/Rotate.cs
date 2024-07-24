using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.Playables;

public class Rotate : MonoBehaviour
{
    [SerializeField] private GameObject gear;
    [SerializeField] private TextMeshProUGUI[] attackCounts = new TextMeshProUGUI[6];
    [SerializeField] private float rotationAmount = 60.0f;  // ��]�ʁi�x���j
    [SerializeField] private float bpm = 120.0f;  // ��]�ɂ�����BPM
    
    private float rotationDuration;  // ��]�ɂ����鎞��
    private float pauseDuration;  // ��~����
    private int AttackCount = 0;
    public int StartCount { get; set; }
    private bool conditionMet = false;  // �������������ꂽ���ǂ����̃t���O
    private bool isAnimating = false;  // �A�j���[�V���������ǂ����̃t���O

    //2024/06/24���问�󖼁@�N���A�������̃C�x���g�J������\�����邽�߂�Gameobject��ǉ�
    [SerializeField] GameObject eventCamera;


    [SerializeField] GameObject Enemy;
    private atkEnemy atkEnemy;

    [SerializeField] Color textColor;

    private void Start()
    {
        atkEnemy = Enemy.GetComponent<atkEnemy>();
        // �������Ăяo��
        // SetStartCount(100);
    }

    void Update()
    {
        // �������Ăяo��
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     Play(100);  //�G�̍U���������Ƃ��ɌĂяo��
        // }
        // if(AttackCount <= LoadChartData.Data.Notes.Count / 2)
        // {
        //     foreach (var attackCount in attackCounts)
        //     {
        //         attackCount.color = textColor;
        //     }
        // }
    }

    /// <summary>
    /// SetStartCount���Ăяo������ɁB
    /// �U���񐔂������ɓ����B
    /// �U���񐔂����炵����AUI�̉�]������֐�
    /// </summary>
    /// <param name="startCount"></param>
    public void Play()
    {
        if (!isAnimating)
        {
            conditionMet = true;
        }

        // �������������ꂽ�瓮�����J�n
        if (conditionMet)
        {
            AttackCount--;

            // �e�U���񐔂̃e�L�X�g���X�V
            foreach (var attackCount in attackCounts)
            {
                if (attackCount.transform.rotation.z > 0.0f)
                {
                    attackCount.text = AttackCount.ToString();
                }
            }


            RotateGear();
            conditionMet = false;  // ��x���������悤�Ƀt���O�����Z�b�g
        }

        if (AttackCount == 0)
        {
            eventCamera.GetComponent<PlayableDirector>().Play();
           

            atkEnemy.DeactivateAllPartsTrail();
        }
    }

    /// <summary>
    /// �ŏ��ɌĂяo���B
    /// �U���񐔂������ɓ����
    /// </summary>
    /// <param name="startCount"></param>
    public void SetStartCount(int startCount)
    {
        // �e�U���񐔂̃e�L�X�g��������
        foreach (var attackCount in attackCounts)
        {
            attackCount.text = startCount.ToString();
        }
        AttackCount = startCount;
    }

    private void RotateGear()
    {
        if (isAnimating)
            return;

        isAnimating = true;  // �A�j���[�V�����J�n

        // BPM�����ɉ�]���ԂƃC���^�[�o�����v�Z
        float beatDuration = 60.0f / bpm;
        rotationDuration = beatDuration / 2;  // ��]�ɂ����鎞�Ԃ��r�[�g�̔����ɐݒ�
        pauseDuration = beatDuration / 2;  // ��~���Ԃ��r�[�g�̔����ɐݒ�

        Sequence gearSequence = DOTween.Sequence(); //�V�[�P���X���쐬�i�O���[�v���I��

        // ���Ԃ̉�]
        gearSequence.Append(gear.transform.DORotate
            (new Vector3(0, 0, gear.transform.eulerAngles.z + rotationAmount), rotationDuration, RotateMode.FastBeyond360));

        // �e�e�L�X�g�̉�]
        foreach (var attackCount in attackCounts)
        {
            gearSequence.Join(attackCount.transform.DORotate
                (new Vector3(0, 0, attackCount.transform.eulerAngles.z + rotationAmount), rotationDuration, RotateMode.FastBeyond360));
        }

        gearSequence.AppendInterval(pauseDuration);   // �C���^�[�o��
        gearSequence.OnComplete(() => { isAnimating = false; conditionMet = false; });

        // �V�[�P���X���Đ�
        gearSequence.Play();
    }
}
