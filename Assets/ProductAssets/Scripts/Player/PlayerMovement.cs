using System.Collections;
using System.Collections.Generic;
using Palmmedia.ReportGenerator.Core;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    // ============= �� �� ===================
    // SE
    private enum SEKind
    {
        Jump_B = 0,
        Jump_S,
        SE_MAX
    }

    private readonly string[] avoidKind = new string[]{
        "UpSmall",
        "DownSmall",
        "leftSmall",
        "rightSmall",
        "UpBig",
        "DownBig",
        "leftBig",
        "rightBig",
    };

    // ============== �R���|�[�l���g ====================
    [SerializeField, NamedArray(typeof(SEKind))] 
    AudioClip[] moveSE = new AudioClip[(int)SEKind.SE_MAX];  // �����SE
    AudioSource audioSource;
    Animator animator;

    EffectPlay effect;

    // =================================================

    // ================ �v���p�e�B =====================
    public EffectPlay Effect
    {
        get { return effect; }
        private set { effect = value; }
    }
    // =================================================

    // =============== �I�u�W�F�N�g�f�[�^ ============
    [SerializeField, NamedArray(typeof(SEKind)), Range(0.0f, 1.0f)]
    float[] SEVolume = new float[(int)SEKind.SE_MAX];     // SE�̉���
    [SerializeField]Transform enemy;

    Vector3[] movePoint;         // �ړ�����_
    int nextPoint;               // ���Ɉړ�����|�C���g
    int nowPoint;                // ���݃v���C���[������|�C���g

    Vector3 centerPos;              // ���p�`�̒��S���W
    Vector3 old_CenterPos;          // ���p�`�̒��S���W
    Vector3 defScale;               // �v���C���[�̏����X�P�[��

    [SerializeField] ushort SPLIT_NUM = 0;     // �ړ��|�C���g�̐�
    [SerializeField] float radius = 0;         // �G�Ƃ̋���
    float moveSpeed = 0.0f;                     
    bool isMove = false;

    // Start is called before the first frame update
    void Start()
    {
        // �[�[�[�[�[�[�[�[�[�[ ������ �[�[�[�[�[�[�[�[�[�[�[�[
        movePoint = new Vector3[SPLIT_NUM];
        old_CenterPos = new Vector3(0.0f, -999999.9f, 0.0f);
        centerPos = Vector3.zero;
        nowPoint = nextPoint = 0;
        // �[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[
        
        // �[�[�[�[�[�[�[�[�[ Settings �[�[�[�[�[�[�[�[�[�[�[�[
        animator = GetComponent<Animator>();
        ComputeMovePoint();
        transform.position = movePoint[nextPoint];
        defScale = transform.localScale;
        audioSource = GetComponent<AudioSource>();  
        effect = gameObject.GetComponent<EffectPlay>();
        // �[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // ���݂̃v���C���[�̈ʒu��ۑ�
        nowPoint = nextPoint;

        ComputeMovePoint();
        // �A�j���[�V������8���Đ������܂ł�
        // ���̉�����󂯕t���Ȃ�
        if (stateInfo.normalizedTime >= 0.8f && !animator.IsInTransition(0))
        {
            // �L�[�ɉ�����������s��
            Avoid(LoadChartData.Data.Notes[GameManager.DataIdx].Kind);
        }

        if (IsAnimationPlaying(animator, "idle"))
            transform.localScale = defScale;
    }

    void FixedUpdate()
    {
        // ���ۂɃv���C���[���ړ������鏈��
        if (nowPoint != nextPoint || transform.position != movePoint[nextPoint])
        {
            transform.position = Vector3.MoveTowards(transform.position, movePoint[nextPoint], moveSpeed * Time.deltaTime);
            isMove = true;
        }
        else
        {
            isMove = false;
        }

        transform.LookAt(enemy);
    }

    /// <summary>
    /// 
    /// ���S���W�����ɑ��p�`�̒��_(�ړ��|�C���g)���v�Z����
    /// 
    /// </summary>
    void ComputeMovePoint()
    {
        if (old_CenterPos != centerPos)
        {
            radius /= 100;
              
            for (int i = 0; i < SPLIT_NUM; ++i)
            {
                // �G�𒆐S�Ƃ������p�`�̒��_���v�Z
                movePoint[i] = new Vector3(
                    centerPos.x + radius * Mathf.Cos(2 * Mathf.PI * i / SPLIT_NUM),
                    centerPos.y,
                    centerPos.z + radius * Mathf.Sin(2 * Mathf.PI * i / SPLIT_NUM));
            }

            old_CenterPos = centerPos;
        }
    }

    /// <summary>
    /// �A�j���[�V�������Đ�����Ă��邩�Đ����܂�
    /// </summary>
    /// <param animator="animator">�A�j���[�^�[</param>
    /// <param animName="animName">�A�j���[�V�����̖��O</param>
    /// <returns>�Đ������ǂ���</returns>
    bool IsAnimationPlaying(Animator animator, string animName)
    {
        if (animator == null)
            return false;

        AnimatorStateInfo info =
            animator.GetCurrentAnimatorStateInfo(0);

        return info.IsName(animName) && animator.GetCurrentAnimatorClipInfo(0).Length > 0;
    }


    void PlaySE(AudioClip[] se, float[] volume, int index)
    {
        if (!audioSource.isPlaying) // SE���Đ�����ĂȂ�������
        {
            audioSource.PlayOneShot(se[index]);
            audioSource.volume = volume[index];
        }
    }
    
    /// <summary>
    /// �v���C���[�̉������
    /// </summary>
    /// <param name="key">���̓L�[</param>
    void Avoid(in LoadChartData.NoteKind kind)
    {
        if (isMove) return; // �����Ă鎞
        if (!RhythmDetect.IsJudging) return; // �m�[�c���莞�Ԃł͂Ȃ��Ƃ�

        //--- ��ނɉ���������A�j���[�V���������s����
        var start = (int)kind < 4 ? 0 : 4;
        var end = (int)kind < 4 ? 4 : 8;
        for (int i = start; i < end; ++i)
        {
            if (MyInput.GetKeyDown((LoadChartData.NoteKind)i) &&
                !IsAnimationPlaying(animator, avoidKind[(int)kind]))
            {
                animator.SetTrigger(avoidKind[(int)kind]);

                // �e��ނ��Ƃ̏���
                switch ((LoadChartData.NoteKind)i)
                {
                    // ����
                    case LoadChartData.NoteKind.E_UP_S:
                    case LoadChartData.NoteKind.E_UP_L:
                        // �G�t�F�N�g�Đ�
                        effect.InstantiateEffect("jump");
                        goto done;

                    // �����
                    case LoadChartData.NoteKind.E_DOWN_S:
                    case LoadChartData.NoteKind.E_DOWN_L:
                        goto done;

                    // �E���
                    case LoadChartData.NoteKind.E_RIGHT_L:
                        transform.localScale = new Vector3(defScale.x * -1, defScale.y, defScale.z);
                        nextPoint += 2;
                        goto done;
                    case LoadChartData.NoteKind.E_RIGHT_S:
                        transform.localScale = new Vector3(defScale.x * -1, defScale.y, defScale.z);
                        nextPoint++;
                        goto done;

                    // �����
                    case LoadChartData.NoteKind.E_LEFT_L:
                        nextPoint -= 2;
                        goto done;
                    case LoadChartData.NoteKind.E_LEFT_S:
                        nextPoint--;
                        goto done;
                }
            }
        }

done:

        // �v���C���[�����[�e�[�V��������悤��
        if (nextPoint > 0)
            nextPoint %= SPLIT_NUM;

        if (nextPoint < 0)
            nextPoint = (SPLIT_NUM - 1) + nextPoint;

        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);


        // �ړ����x�̌v�Z
        float distance = (movePoint[nextPoint] - movePoint[nowPoint]).magnitude;

        moveSpeed = distance / info.length;

    }
}
