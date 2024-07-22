using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class LoadChartData : MonoBehaviour
{
    /// <summary>
    /// ノーツの種類
    /// </summary>
    public enum NoteKind
    {
        // 小
        E_UP_S,
        E_DOWN_S,
        E_LEFT_S,
        E_RIGHT_S,
        // 大
        E_UP_L,
        E_DOWN_L,
        E_LEFT_L,
        E_RIGHT_L,
    }

    /// <summary>
    /// 譜面データ
    /// </summary>
    public struct ChartData
    {
        /// <summary>曲タイトル</summary>
        public string Title { get; set; }
        /// <summary>１分間の拍数</summary>
        public float Bpm { get; set; }

        /// <summary>曲のスタート(s)</summary>
        public float Start { get; set; }

        /// <summary>曲の終わり(s)</summary>
        public float End { get; set; }
        /// <summary>ノーツ格納リスト</summary>
        public List<Note> Notes { get; set; }
    }

    /// <summary>
    /// ノーツデータ
    /// </summary>
    public struct Note
    {
        /// <summary>種類</summary>
        public NoteKind Kind { get; set; }
        /// <summary>時間</summary>
        public float Time { get; set; }
        /// <summary>スタイリッシュカメラが有効？</summary>
        public bool IsEnableCamera { get; set; }
        /// <summary>ノーツが攻撃予測？</summary>
        public bool IsAtkPredict { get; set; }
    }

    //--- 変数宣言 ---
    /// <summary> 譜面データ </summary>
    public static ChartData Data = new ChartData();// ゲーム中一つずつしか読み込まないのでstaticで保持


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
    /// 譜面データ読み込み
    /// </summary>
    public static void Load(in int fileNo)
    {
        // 初期化
        Data = new ChartData();
        Data.Notes = new List<Note>();

        var text = File.ReadAllText(Application.dataPath + "/" + "ProductAssets/Data/Stage" + fileNo.ToString() + ".json");

        Data = JsonConvert.DeserializeObject<ChartData>(text);
    }

    //--- サブルーチン
 }


