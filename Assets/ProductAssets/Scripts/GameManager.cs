using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using EasyTransition;
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

    public bool isGameStarted = false;

    [SerializeField] TransitionSettings transition;
    [SerializeField] float loaddelay;    

    [SerializeField] GameObject AtkCounterUI;

    [SerializeField] float FadeoutTime = 10.0f;

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

    //2024/07/18・ｽ・ｽ・ｽ髣ｮ・ｽ・ｽ
    //・ｽX・ｽ^・ｽ[・ｽg・ｽJ・ｽ・ｽ・ｽ・ｽ・ｽ・ｽ・ｽI・ｽ・ｽ・ｽ・ｽ・ｽ・ｽ・ｽ迚ｹ・ｽy・ｽ・ｽ・ｽﾄ撰ｿｽ・ｽ・ｽ・ｽ驍ｽ・ｽﾟにス・ｽ^・ｽ[・ｽg・ｽJ・ｽ・ｽ・ｽ・ｽ・ｽ・ｽgameobject・ｽ・ｽﾇ会ｿｽ
    [SerializeField] GameObject startCamera;
    //2024/07/19・ｽ・ｽ・ｽ髣ｮ・ｽ・ｽ
    //・ｽ・ｽ・ｽX・ｽg・ｽJ・ｽ・ｽ・ｽ・ｽ・ｽﾌ遷・ｽﾚゑｿｽ・ｽI・ｽ・ｽ・ｽ・ｽ・ｽ・ｽ・ｽ・ｽV・ｽ[・ｽ・ｽ・ｽﾘゑｿｽﾖゑｿｽ・ｽ・ｽ・ｽ・ｽ・ｽ驍ｽ・ｽﾟに・ｿｽ・ｽX・ｽg・ｽJ・ｽ・ｽ・ｽ・ｽ・ｽ・ｽgameobject・ｽ・ｽﾇ会ｿｽ
    [SerializeField] GameObject lastCamera;

    DamageFlash dmgFlash;
    AudioSource BGM;
    void Start()
    {

        LoadChartData.Load(0/*?????V?[????????K?v?f?[?^?????[?h*/);

        cameraScript = GetComponent<CameraChange>();
        rotate = GetComponent<Rotate>();

        rotate.SetStartCount(LoadChartData.Data.Notes.Count / 2);

        BGM = GetComponent<AudioSource>();
        BGM.time = LoadChartData.Data.Start;

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

        //・ｽI・ｽ・ｽ骼橸ｿｽﾔゑｿｽ・ｽtimeline・ｽ・ｽ・ｽﾄ撰ｿｽ・ｽ・ｽ・ｽ・ｽﾄゑｿｽ・ｽ・ｽ・ｽ・ｽ・ｽ・ｽ・ｽs
        if (!isGameStarted && startCamera.GetComponent<PlayableDirector>().duration - 0.2f <= startCamera.GetComponent<PlayableDirector>().time)
        {
            isGameStarted = true;

            AtkCounterUI.SetActive(true);

            BGM.Play();
        }

        // ?J?E???g
        if (BGM.isPlaying)
        {
            second = BGM.time;

            if (debugText) debugText.text = second.ToString();

        }


        if (!isFinish)
        {

            // ???・ｽ・ｽ??I??????????U??
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
            //・ｽ・ｽ・ｽX・ｽg・ｽA・ｽ^・ｽb・ｽN・ｽ・ｽﾊに遷・ｽﾚゑｿｽ・ｽ・ｽO・ｽﾉス・ｽR・ｽA・ｽ翌・ｽ
            S_Result.SetScore(stylishCnt, normalCnt, badCnt);

            StartCoroutine(FadeOutBGM(BGM, FadeoutTime));

            TransLoadScene("LastAttackScene");
            
            ////・ｽI・ｽ・ｽ骼橸ｿｽﾔゑｿｽ・ｽtimeline・ｽ・ｽ・ｽﾄ撰ｿｽ・ｽ・ｽ・ｽ・ｽﾄゑｿｽ・ｽ・ｽ・ｽ・ｽ・ｽ・ｽ・ｽs
            //if (lastCamera.GetComponent<PlayableDirector>().duration - 0.1f <= lastCamera.GetComponent<PlayableDirector>().time)
            //{
            //    //・ｽ・ｽ・ｽU・ｽ・ｽ・ｽg・ｽ・ｽﾊに遷・ｽﾚゑｿｽ・ｽ・ｽO・ｽﾉス・ｽR・ｽA・ｽ翌・ｽ
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

                // ・ｽX・ｽ^・ｽC・ｽ・ｽ・ｽb・ｽV・ｽ・ｽ・ｽJ・ｽ・ｽ・ｽ・ｽ・ｽ・ｽ・ｽL・ｽ・ｽ・ｽﾈノ・ｽ[・ｽc・ｽﾈゑｿｽﾘゑｿｽﾖゑｿｽ・ｽ・ｽ
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

    void TransLoadScene(string _scenename)
    {
        TransitionManager.Instance().Transition(_scenename,transition,loaddelay);
    }    

    IEnumerator FadeOutBGM(AudioSource audioSource, float fadeDuration)
    {
        float startVolume = audioSource.volume;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0, elapsedTime / fadeDuration);
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}