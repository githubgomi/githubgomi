using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class atkEnemy : MonoBehaviour
{
    // 定数
    private enum SEKind
    {
        Sweep = 0,
        SwingDown,
        SE_MAX
    }

    // コンポーネント
    [SerializeField] Transform player;
    [SerializeField] GameObject[] bodyParts_Trail; //攻撃の軌跡を表示させるための変数
    [SerializeField, NamedArray(typeof(SEKind))]
    AudioClip[] atkSE = new AudioClip[(int)SEKind.SE_MAX];
    AudioSource audioSource;
    Animator animator;

    [SerializeField, NamedArray(typeof(SEKind)), Range(0.0f, 1.0f)]
    float[] SEValue = new float[(int)SEKind.SE_MAX];  // SEの音量
    int oldAttack;
    bool isAttack;

    //攻撃の軌跡を表示する手足の名前
    private enum bodyPartsKind
    {
        L_HAND  = 0,    //左手
        R_HAND  = 1,    //右手
        L_LEG  = 2,     //左足
        R_LEG  = 3,     //右足
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
        // 攻撃位置を確認する
        if (GameManager.Second > LoadChartData.Data.Notes[GameManager.DataIdx].Time)
        {
            if (oldAttack != GameManager.DataIdx)
            {
                oldAttack = GameManager.DataIdx;

                // 攻撃予測データを確認する
                if (LoadChartData.Data.Notes[GameManager.DataIdx].IsAtkPredict)
                {
                    isAttack = true;
                    // ノートを生成
                    NoteManager.CreateNote(LoadChartData.Data.Notes[GameManager.DataIdx].Kind, true,GameManager.DataIdx);

                    // 攻撃予測→攻撃(４拍)
                    // 敵の攻撃モーション開始から攻撃タイミングまで２拍
                    // １拍分の秒数、予測攻撃から遅らせてモーションを開始する
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
                DeactivateAllPartsTrail(); //前回表示した軌跡を非表示に戻しとく

                animator.SetTrigger("B_UP");
                // SE再生
                //PlaySE(atkSE, SEValue, (int)SEKind.SwingDown);
                ActivePartsTrail(bodyPartsKind.L_HAND);
                ActivePartsTrail(bodyPartsKind.R_HAND);
                break;

            case LoadChartData.NoteKind.E_UP_S:
                DeactivateAllPartsTrail();

                animator.SetTrigger("S_UP");
                // SE再生
                //PlaySE(atkSE, SEValue, (int)SEKind.SwingDown);
                ActivePartsTrail(bodyPartsKind.R_HAND);
                break;

            case LoadChartData.NoteKind.E_DOWN_L:
                DeactivateAllPartsTrail();

                animator.SetTrigger("B_Down");
                // SE再生
                //PlaySE(atkSE, SEValue, (int)SEKind.SwingDown);
                ActivePartsTrail(bodyPartsKind.L_HAND);
                ActivePartsTrail(bodyPartsKind.R_HAND);
                break;

            case LoadChartData.NoteKind.E_DOWN_S:
                DeactivateAllPartsTrail();

                animator.SetTrigger("S_Down");
                // SE再生
                //PlaySE(atkSE, SEValue, (int)SEKind.SwingDown);
                ActivePartsTrail(bodyPartsKind.L_HAND);
                break;

            case LoadChartData.NoteKind.E_RIGHT_L:
                DeactivateAllPartsTrail();

                animator.SetTrigger("B_Right");
                // SE再生
                //PlaySE(atkSE, SEValue, (int)SEKind.Sweep);
                ActivePartsTrail(bodyPartsKind.L_HAND);
                ActivePartsTrail(bodyPartsKind.R_HAND);
                break;

            case LoadChartData.NoteKind.E_RIGHT_S:
                DeactivateAllPartsTrail();

                animator.SetTrigger("S_Right");
                // SE再生
                //PlaySE(atkSE, SEValue, (int)SEKind.Sweep);
                ActivePartsTrail(bodyPartsKind.L_HAND);
                ActivePartsTrail(bodyPartsKind.L_LEG);
                break;

            case LoadChartData.NoteKind.E_LEFT_L:
                DeactivateAllPartsTrail();

                animator.SetTrigger("B_Left");
                // SE再生
                //PlaySE(atkSE, SEValue, (int)SEKind.Sweep);
                ActivePartsTrail(bodyPartsKind.L_HAND);
                ActivePartsTrail(bodyPartsKind.R_HAND);
                break;

            case LoadChartData.NoteKind.E_LEFT_S:
                DeactivateAllPartsTrail();

                animator.SetTrigger("S_Left");
                // SE再生
                //PlaySE(atkSE, SEValue, (int)SEKind.Sweep);
                ActivePartsTrail(bodyPartsKind.L_HAND);
                ActivePartsTrail(bodyPartsKind.L_LEG);
                break;
        }
    }
}
