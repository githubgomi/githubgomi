using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RhythmDetect : MonoBehaviour
{
    [SerializeField] float StylishRange;
    [SerializeField] float GoodRange;
    [SerializeField] AudioClip audioClip;

    

    /// <summary> スタイリッシュ回避の判定秒数 </summary>
    public static float STYLISH_RANGE { get; set; }

    /// <summary> 普通回避の判定秒数</summary>
    public static float GOOD_RANGE { get; set; }

    public static float DODGE_RANGE { get; set; }

    static bool isInput = false;


    /// <summary>
    /// 判定中かどうか
    /// </summary>
    public static bool IsInput
    {
        get { return isInput; }
        private set { isInput = value; }
    }

    /// <summary>
    /// 現在の再生時間がノーツ判定か？
    /// </summary>
    public static bool IsJudging {  get; private set; }

    /// <summary>
    /// 判定されたタイミングの種類
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
            // ノーツの判定中かを取得
            var time = LoadChartData.Data.Notes[GameManager.DataIdx].Time;
            IsJudging = (time - GoodRange < GameManager.Second && time + GoodRange > GameManager.Second);
        }

    }

    /// <summary>
    /// 判定をとる
    /// </summary>
    /// <param name="justTiming"></param>
    /// <param name="kind">回避種類</param>
    /// <returns></returns>
    public static void DodgeDetect(in float justTiming, in LoadChartData.NoteKind kind)
    {
        if (isInput) return;

        // 正しいキーをチェック
        if (MyInput.GetKeyDown(kind))
        {
            isInput = true;
            CheckRanged(justTiming, GameManager.Second);
        }
        else
        {
            // 判定とは違うキーが押されていないかチェック
            // 回避の種類が小か大かでキー入力も小か大しか判定しない
            for (int i = (int)kind < 4 ? 0 : 4;
                i < ((int)kind > 4 ? 4 : 8);
                i++)
            {
                if (MyInput.GetKeyDown((LoadChartData.NoteKind)i))
                {
                    isInput = true;
                    result = Dodge.miss;
                    Debug.Log("入力キーが違います");
                    return;
                }
            }
        }

        // 指定時間内に特定の入力がない
        if (!isInput && GameManager.Second > justTiming + GOOD_RANGE)
        {
            isInput = true;
            Debug.Log("no key");
            result = Dodge.miss;
        }
    }


    //--- サブルーチン

    /// <summary>
    /// 判定レンジで入力されたか結果を格納します
    /// </summary>
    /// <param name="justTiming">判定が行われる時間</param>
    /// <param name="currentTime">現在の時間</param>
    static void CheckRanged(in float justTiming, in float currentTime)
    {
        // 判定時間との誤差を求める
        float timeDifference = Mathf.Abs(justTiming - currentTime);

        Debug.Log(timeDifference);

        // 誤差からどのレンジにいるか調べる
        if (timeDifference <= STYLISH_RANGE)
            result = Dodge.stylish;

        else if (timeDifference <= GOOD_RANGE)
            result = Dodge.good;

        else
        {
            Debug.Log("遅い");
            result = Dodge.miss;
        }
    }

    /// <summary>
    /// 正しいキーがおされたか判定
    /// </summary>
    /// <param name="key">正しいキー</param>
    /// <param name="sucsess">成功時の回避</param>
    /// <returns>判定結果</returns>
    static Dodge KeyChecker(Dodge sucsess, KeyCode key1, KeyCode key2)
    {
        //--- 判定キーが１つ
        if(key2 == KeyCode.None)
        {
            // 正しいキーがおされた？
            if (Input.GetKeyDown(key1))
            {
                isInput = true;
                return sucsess;
            
            }
            // 間違ったキーがおされた？
            else if (Input.anyKeyDown)
            {
                isInput = true;
                return Dodge.miss;
            }
            // キーが押されなかった
            return Dodge.none;
        }
        else
        {
            // 判定キーが２つ
            if (Input.GetKey(key1) && Input.GetKeyDown(key2) ||
                Input.GetKeyDown(key1) && Input.GetKey(key2))
            {
                isInput = true;
                return sucsess;
            }
            // 間違ったキーがおされた？
            else if (!Input.GetKey(key1) && Input.anyKeyDown)
            {
                isInput = true;
                Debug.Log("miss key");
                return Dodge.miss;
            }
            // キーが押されなかった
            return Dodge.none;
        }
    }

    /// <summary>
    /// 引数で指定したキーのどれか１つでも
    /// 入力されているか判定する
    /// </summary>
    /// <param name="keys">[params] 入力判定を行うキー</param>
    /// <returns>入力がされたか</returns>
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
