using UnityEngine;


public class S_Pause : MonoBehaviour
{
    // Start is called before the first frame update

    //ポーズ画面のプレハブを持ってくる
    [SerializeField] private GameObject pauseUI;

    //ポーズ画面中か確認(trueでポーズ中)
    private bool bNowPause = false;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //キーを押すとbNowPauseを反転
        if (Input.GetKeyDown(KeyCode.Escape))
            bNowPause = !bNowPause;

       //bNowPauseに応じてポーズ画面に変更するか決める
        if (bNowPause)
            Pause();
        else
            GameScene();
    }

    private void FixedUpdate()
    {
        //Debug.Log("経過時間(秒)" + Time.time);
    }

    private void Pause()
    {
        //Pause画面が表示されているか確認する。
        if (!pauseUI.activeSelf)
        {
            //アクティブに変更
            pauseUI.SetActive(true);
            //ゲームの時間を止める
            Time.timeScale = 0f;
        }

    }

   private void GameScene()
    {
        //Pause画面が表示されていたら非表示にする
        if (pauseUI.activeInHierarchy)
        {
            pauseUI.SetActive(false);
            Time.timeScale = 1f;
        }
    }


    //Pause中、リスタートボタンが押されたときにbNowPauseをfalseにする
    public void ReStart()
    {
        if (bNowPause)
            bNowPause = false;
    }

}
