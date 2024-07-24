using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.Playables;

public class Rotate : MonoBehaviour
{
    [SerializeField] private GameObject gear;
    [SerializeField] private TextMeshProUGUI[] attackCounts = new TextMeshProUGUI[6];
    [SerializeField] private float rotationAmount = 60.0f;  // 回転量（度数）
    [SerializeField] private float bpm = 120.0f;  // 回転にかかるBPM
    
    private float rotationDuration;  // 回転にかかる時間
    private float pauseDuration;  // 停止時間
    private int AttackCount = 0;
    public int StartCount { get; set; }
    private bool conditionMet = false;  // 条件が満たされたかどうかのフラグ
    private bool isAnimating = false;  // アニメーション中かどうかのフラグ

    //2024/06/24金城琉希名　クリアした時のイベントカメラを表示するためにGameobjectを追加
    [SerializeField] GameObject eventCamera;


    [SerializeField] GameObject Enemy;
    private atkEnemy atkEnemy;

    [SerializeField] Color textColor;

    private void Start()
    {
        atkEnemy = Enemy.GetComponent<atkEnemy>();
        // お試し呼び出し
        // SetStartCount(100);
    }

    void Update()
    {
        // お試し呼び出し
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     Play(100);  //敵の攻撃が来たときに呼び出す
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
    /// SetStartCountを呼び出した後に。
    /// 攻撃回数を引数に入れる。
    /// 攻撃回数を減らしたり、UIの回転をする関数
    /// </summary>
    /// <param name="startCount"></param>
    public void Play()
    {
        if (!isAnimating)
        {
            conditionMet = true;
        }

        // 条件が満たされたら動きを開始
        if (conditionMet)
        {
            AttackCount--;

            // 各攻撃回数のテキストを更新
            foreach (var attackCount in attackCounts)
            {
                if (attackCount.transform.rotation.z > 0.0f)
                {
                    attackCount.text = AttackCount.ToString();
                }
            }


            RotateGear();
            conditionMet = false;  // 一度だけ動くようにフラグをリセット
        }

        if (AttackCount == 0)
        {
            eventCamera.GetComponent<PlayableDirector>().Play();
           

            atkEnemy.DeactivateAllPartsTrail();
        }
    }

    /// <summary>
    /// 最初に呼び出す。
    /// 攻撃回数を引数に入れる
    /// </summary>
    /// <param name="startCount"></param>
    public void SetStartCount(int startCount)
    {
        // 各攻撃回数のテキストを初期化
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

        isAnimating = true;  // アニメーション開始

        // BPMを元に回転時間とインターバルを計算
        float beatDuration = 60.0f / bpm;
        rotationDuration = beatDuration / 2;  // 回転にかかる時間をビートの半分に設定
        pauseDuration = beatDuration / 2;  // 停止時間もビートの半分に設定

        Sequence gearSequence = DOTween.Sequence(); //シーケンスを作成（グループ化的な

        // 歯車の回転
        gearSequence.Append(gear.transform.DORotate
            (new Vector3(0, 0, gear.transform.eulerAngles.z + rotationAmount), rotationDuration, RotateMode.FastBeyond360));

        // 各テキストの回転
        foreach (var attackCount in attackCounts)
        {
            gearSequence.Join(attackCount.transform.DORotate
                (new Vector3(0, 0, attackCount.transform.eulerAngles.z + rotationAmount), rotationDuration, RotateMode.FastBeyond360));
        }

        gearSequence.AppendInterval(pauseDuration);   // インターバル
        gearSequence.OnComplete(() => { isAnimating = false; conditionMet = false; });

        // シーケンスを再生
        gearSequence.Play();
    }
}
