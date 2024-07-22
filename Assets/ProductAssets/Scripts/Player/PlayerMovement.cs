using System.Collections;
using System.Collections.Generic;
using Palmmedia.ReportGenerator.Core;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    // ============= 定 数 ===================
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

    // ============== コンポーネント ====================
    [SerializeField, NamedArray(typeof(SEKind))] 
    AudioClip[] moveSE = new AudioClip[(int)SEKind.SE_MAX];  // 回避のSE
    AudioSource audioSource;
    Animator animator;

    EffectPlay effect;

    // =================================================

    // ================ プロパティ =====================
    public EffectPlay Effect
    {
        get { return effect; }
        private set { effect = value; }
    }
    // =================================================

    // =============== オブジェクトデータ ============
    [SerializeField, NamedArray(typeof(SEKind)), Range(0.0f, 1.0f)]
    float[] SEVolume = new float[(int)SEKind.SE_MAX];     // SEの音量
    [SerializeField]Transform enemy;

    Vector3[] movePoint;         // 移動する点
    int nextPoint;               // 次に移動するポイント
    int nowPoint;                // 現在プレイヤーがいるポイント

    Vector3 centerPos;              // 多角形の中心座標
    Vector3 old_CenterPos;          // 多角形の中心座標
    Vector3 defScale;               // プレイヤーの初期スケール

    [SerializeField] ushort SPLIT_NUM = 0;     // 移動ポイントの数
    [SerializeField] float radius = 0;         // 敵との距離
    float moveSpeed = 0.0f;                     
    bool isMove = false;

    // Start is called before the first frame update
    void Start()
    {
        // ーーーーーーーーーー 初期化 ーーーーーーーーーーーー
        movePoint = new Vector3[SPLIT_NUM];
        old_CenterPos = new Vector3(0.0f, -999999.9f, 0.0f);
        centerPos = Vector3.zero;
        nowPoint = nextPoint = 0;
        // ーーーーーーーーーーーーーーーーーーーーーーーーーー
        
        // ーーーーーーーーー Settings ーーーーーーーーーーーー
        animator = GetComponent<Animator>();
        ComputeMovePoint();
        transform.position = movePoint[nextPoint];
        defScale = transform.localScale;
        audioSource = GetComponent<AudioSource>();  
        effect = gameObject.GetComponent<EffectPlay>();
        // ーーーーーーーーーーーーーーーーーーーーーーーーーー
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // 現在のプレイヤーの位置を保存
        nowPoint = nextPoint;

        ComputeMovePoint();
        // アニメーションが8割再生されるまでは
        // 次の回避を受け付けない
        if (stateInfo.normalizedTime >= 0.8f && !animator.IsInTransition(0))
        {
            // キーに応じた回避を行う
            Avoid(LoadChartData.Data.Notes[GameManager.DataIdx].Kind);
        }

        if (IsAnimationPlaying(animator, "idle"))
            transform.localScale = defScale;
    }

    void FixedUpdate()
    {
        // 実際にプレイヤーを移動させる処理
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
    /// 中心座標を元に多角形の頂点(移動ポイント)を計算する
    /// 
    /// </summary>
    void ComputeMovePoint()
    {
        if (old_CenterPos != centerPos)
        {
            radius /= 100;
              
            for (int i = 0; i < SPLIT_NUM; ++i)
            {
                // 敵を中心とした多角形の頂点を計算
                movePoint[i] = new Vector3(
                    centerPos.x + radius * Mathf.Cos(2 * Mathf.PI * i / SPLIT_NUM),
                    centerPos.y,
                    centerPos.z + radius * Mathf.Sin(2 * Mathf.PI * i / SPLIT_NUM));
            }

            old_CenterPos = centerPos;
        }
    }

    /// <summary>
    /// アニメーションが再生されているか再生します
    /// </summary>
    /// <param animator="animator">アニメーター</param>
    /// <param animName="animName">アニメーションの名前</param>
    /// <returns>再生中かどうか</returns>
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
        if (!audioSource.isPlaying) // SEが再生されてなかったら
        {
            audioSource.PlayOneShot(se[index]);
            audioSource.volume = volume[index];
        }
    }
    
    /// <summary>
    /// プレイヤーの回避処理
    /// </summary>
    /// <param name="key">入力キー</param>
    void Avoid(in LoadChartData.NoteKind kind)
    {
        if (isMove) return; // 動いてる時
        if (!RhythmDetect.IsJudging) return; // ノーツ判定時間ではないとき

        //--- 種類に応じた回避アニメーションを実行する
        var start = (int)kind < 4 ? 0 : 4;
        var end = (int)kind < 4 ? 4 : 8;
        for (int i = start; i < end; ++i)
        {
            if (MyInput.GetKeyDown((LoadChartData.NoteKind)i) &&
                !IsAnimationPlaying(animator, avoidKind[(int)kind]))
            {
                animator.SetTrigger(avoidKind[(int)kind]);

                // 各種類ごとの処理
                switch ((LoadChartData.NoteKind)i)
                {
                    // 上回避
                    case LoadChartData.NoteKind.E_UP_S:
                    case LoadChartData.NoteKind.E_UP_L:
                        // エフェクト再生
                        effect.InstantiateEffect("jump");
                        goto done;

                    // 下回避
                    case LoadChartData.NoteKind.E_DOWN_S:
                    case LoadChartData.NoteKind.E_DOWN_L:
                        goto done;

                    // 右回避
                    case LoadChartData.NoteKind.E_RIGHT_L:
                        transform.localScale = new Vector3(defScale.x * -1, defScale.y, defScale.z);
                        nextPoint += 2;
                        goto done;
                    case LoadChartData.NoteKind.E_RIGHT_S:
                        transform.localScale = new Vector3(defScale.x * -1, defScale.y, defScale.z);
                        nextPoint++;
                        goto done;

                    // 左回避
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

        // プレイヤーがローテーションするように
        if (nextPoint > 0)
            nextPoint %= SPLIT_NUM;

        if (nextPoint < 0)
            nextPoint = (SPLIT_NUM - 1) + nextPoint;

        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);


        // 移動速度の計算
        float distance = (movePoint[nextPoint] - movePoint[nowPoint]).magnitude;

        moveSpeed = distance / info.length;

    }
}
