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

    //2024/07/18�E��E��E�问�E��E�
    //�E�X�E�^�E�[�E�g�E�J�E��E��E��E��E��E��E�I�E��E��E��E��E��E��E�特�E�y�E��E��E�Đ��E��E��E�邽�E�߂ɃX�E�^�E�[�E�g�E�J�E��E��E��E��E��E�gameobject�E��E�ǉ�
    [SerializeField] GameObject startCamera;
    //2024/07/19�E��E��E�问�E��E�
    //�E��E��E�X�E�g�E�J�E��E��E��E��E�̑J�E�ڂ��E�I�E��E��E��E��E��E��E��E�V�E�[�E��E��E�؂�ւ��E��E��E��E��E�邽�E�߂ɁE���E�X�E�g�E�J�E��E��E��E��E��E�gameobject�E��E�ǉ�
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

        //�E�I�E��E�鎞�Ԃ��E�timeline�E��E��E�Đ��E��E��E��E�Ă��E��E��E��E��E��E��E�s
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

            // ???�E��E�??I??????????U??
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
            //�E��E��E�X�E�g�E�A�E�^�E�b�E�N�E��E�ʂɑJ�E�ڂ��E��E�O�E�ɃX�E�R�E�A�E�𑗂�E�
            S_Result.SetScore(stylishCnt, normalCnt, badCnt);

            StartCoroutine(FadeOutBGM(BGM, FadeoutTime));

            TransLoadScene("LastAttackScene");
            
            ////�E�I�E��E�鎞�Ԃ��E�timeline�E��E��E�Đ��E��E��E��E�Ă��E��E��E��E��E��E��E�s
            //if (lastCamera.GetComponent<PlayableDirector>().duration - 0.1f <= lastCamera.GetComponent<PlayableDirector>().time)
            //{
            //    //�E��E��E�U�E��E��E�g�E��E�ʂɑJ�E�ڂ��E��E�O�E�ɃX�E�R�E�A�E�𑗂�E�
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

                Debug.Log($"����{LoadChartData.Data.Notes[GameManager.DataIdx].Kind} : {GameManager.Second}");
                break;

            case RhythmDetect.Dodge.stylish:
                temp.Effect.InstantiateEffect("stylishUI");

                // �E�X�E�^�E�C�E��E��E�b�E�V�E��E��E�J�E��E��E��E��E��E��E�L�E��E��E�ȃm�E�[�E�c�E�Ȃ�؂�ւ��E��E�
                if (LoadChartData.Data.Notes[DataIdx].IsEnableCamera) { 
                    cameraScript.ChangeCam(LoadChartData.Data.Notes[DataIdx].Kind);
                }

                temp.Effect.InstantiateEffect("stylish", new Vector3(0, 1, 0));
                stylishCnt++;


                Debug.Log($"����{LoadChartData.Data.Notes[GameManager.DataIdx].Kind} : {GameManager.Second}");

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