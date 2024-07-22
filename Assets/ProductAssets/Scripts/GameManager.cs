using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.Playables;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{

    // ============ ?? ===============
    readonly KeyCode[] key1 = {
        KeyCode.W,
        KeyCode.S,
        KeyCode.A,
        KeyCode.D,
        KeyCode.W,
        KeyCode.S,
        KeyCode.A,
        KeyCode.D,
    };

    readonly KeyCode[] key2 = {
        KeyCode.None,
        KeyCode.None,
        KeyCode.None,
        KeyCode.None,
        KeyCode.LeftShift,
        KeyCode.LeftShift,
        KeyCode.LeftShift,
        KeyCode.LeftShift,
    };


    static float second = 0.0f;  // ???y?o?????

    public static float Second
    {
        get { return second; }
        private set {; }
    }
    [SerializeField] Text debugText;
    RhythmDetect.Dodge res;    // ????
    static int dataIdx = 0;    // ?U????
    static int attackIdx = -1; // ?????U??????index??? 

    public TextMesh text;
    public Text DodgeResText;

    [SerializeField]
    GameObject player;

    CameraChange cameraScript;
    Rotate rotate;

    float stylishCnt = 0.0f;
    float normalCnt = 0.0f;
    float badCnt = 0.0f;

    scoreUI scoreUI;


    /// <summary>
    /// ?????U???f?[?^?Q??p
    /// </summary>
    static public int DataIdx
    {
        get { return dataIdx; }
        private set {; }
    }

    static public int AttackIdx
    {
        get { return attackIdx; }
        set { attackIdx = value; }
    }

    bool isFinish = false; // ?X?e?[?W?I???t???O

    //2024/07/18金城琉希名
    //スタートカメラが終了したら音楽を再生するためにスタートカメラのgameobjectを追加
    [SerializeField] GameObject startCamera;
    //2024/07/19金城琉希名
    //ラストカメラの遷移が終了したらシーン切り替えをするためにラストカメラのgameobjectを追加
    [SerializeField] GameObject lastCamera;

    DamageFlash dmgFlash;

    AudioSource audio;
    void Start()
    {
        LoadChartData.Load(0/*?????V?[????????K?v?f?[?^?????[?h*/);

        cameraScript = GetComponent<CameraChange>();
        rotate = GetComponent<Rotate>();

        rotate.SetStartCount(LoadChartData.Data.Notes.Count / 2);

        audio = GetComponent<AudioSource>();
        audio.time = LoadChartData.Data.Start;

        attackIdx = dataIdx;

        isFinish = false;
        stylishCnt = 0.0f;
        normalCnt = 0.0f;
        badCnt = 0.0f;

        dmgFlash = player.transform.GetChild(0).GetComponent<DamageFlash>();
    }

    // Update is called once per frame
    void Update()
    {
        //?Q?[?????I????????^?C????????
        if (isFinish) Destroy(debugText);

        //終わる時間よりtimelineが再生されていた時実行
        if (startCamera.GetComponent<PlayableDirector>().duration - 0.2f <= startCamera.GetComponent<PlayableDirector>().time)
        {
            audio.Play();
        }

        // ?J?E???g
        if (audio.isPlaying)
        {
            second = audio.time;

            if (debugText) debugText.text = second.ToString();

        }


        if (!isFinish)
        {

            // ???莞??I??????????U??
            if (second > LoadChartData.Data.Notes[dataIdx].Time + RhythmDetect.GOOD_RANGE + 0.2f)
            {
                if (!LoadChartData.Data.Notes[dataIdx].IsAtkPredict)
                {
                    attackIdx = dataIdx;
                    rotate.Play();
                }

                // ?S???U?????I?????
                if (dataIdx >= LoadChartData.Data.Notes.Count - 1)
                    isFinish = true;
                else
                    dataIdx++;  // ????U????

            }

            if (!LoadChartData.Data.Notes[dataIdx].IsAtkPredict)
                AttackDodge();
        }
        else
        {
            //ラストアタック画面に遷移する前にスコアを送る
            S_Result.SetScore(stylishCnt, normalCnt, badCnt);
            SceneManager.LoadScene("LastAttackScene");
            ////終わる時間よりtimelineが再生されていた時実行
            //if (lastCamera.GetComponent<PlayableDirector>().duration - 0.1f <= lastCamera.GetComponent<PlayableDirector>().time)
            //{
            //    //リザルト画面に遷移する前にスコアを送る
            //    S_Result.SetScore(stylishCnt, normalCnt, badCnt);
            //    S_Load.SetNextLoadScene("Result");
            //}
        }
    }

    /// <summary>
    /// ?????????s??
    /// </summary>
    void AttackDodge()
    {

        int idx = (int)LoadChartData.Data.Notes[dataIdx].Kind;

        // ?\?????????????s????
        if (LoadChartData.Data.Notes[dataIdx].IsAtkPredict)
            return;

        // ????????
        RhythmDetect.DodgeDetect(
            (float)LoadChartData.Data.Notes[dataIdx].Time,
            LoadChartData.Data.Notes[dataIdx].Kind
        );


        PlayerMovement temp = player.transform.GetChild(0).GetComponent<PlayerMovement>();



        // ??????????????????????
        switch (RhythmDetect.Result)
        {
            default:
                DodgeResText.text = "";
                break;

            case RhythmDetect.Dodge.miss:
                Debug.Log("miss");
                dmgFlash.Flash();
                temp.Effect.InstantiateEffect("notStylishUI");
                badCnt++;
                break;

            case RhythmDetect.Dodge.good:
                temp.Effect.InstantiateEffect("notStylishUI");
                normalCnt++;

                Debug.Log($"判定{LoadChartData.Data.Notes[GameManager.DataIdx].Kind} : {GameManager.Second}");
                break;

            case RhythmDetect.Dodge.stylish:
                temp.Effect.InstantiateEffect("stylishUI");

                // スタイリッシュカメラが有効なノーツなら切り替える
                if (LoadChartData.Data.Notes[DataIdx].IsEnableCamera) { 
                    cameraScript.ChangeCam(LoadChartData.Data.Notes[DataIdx].Kind);
                }

                temp.Effect.InstantiateEffect("stylish", new Vector3(0, 1, 0));
                stylishCnt++;


                Debug.Log($"判定{LoadChartData.Data.Notes[GameManager.DataIdx].Kind} : {GameManager.Second}");

                break;
        }

        scoreUI.AddScore(RhythmDetect.Result);
    }
}