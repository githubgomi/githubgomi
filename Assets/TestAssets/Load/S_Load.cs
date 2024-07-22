using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.UI;


public class S_Load : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject center;//月が回る中心点
    [SerializeField] private GameObject mooonObj;
    [SerializeField] private float radius;//中心点から月の画像までの半径

    private float pi = 3.14159265f;
    //　非同期動作で使用するAsyncOperation
    private AsyncOperation async;

    static string sceneName;
    static float loadTime;

    float time;
    float oldTime;

    private void Start()
    {
        oldTime = Time.time;

        if (loadTime > 3.0f)
        {
            StartCoroutine(LoadData());
        }
        else
        {
            StartCoroutine(WaitLoadData());
        }
    }



    // Update is called once per frame
    private void Update()
    {

        time = Time.time - oldTime;
    }



    public static void SetNextLoadScene(string name)
    {

        //ファイル検索
        // １ギガバイト読み込むのに大体１秒くらいと仮定
        //ここをシーンがあるフォルダのところにパスを設定。そこのフォルダの名前を検索
        var fileInfo = new FileInfo(Application.dataPath + "/" + "ProductAssets/Scenes/" + name + ".unity");

        //ロード時間を仮定
        loadTime = (fileInfo.Length / (1024f * 1024f * 1024));

        sceneName = Path.GetFileNameWithoutExtension(fileInfo.Name);

        //次に読み込むシーンを選択したらロードシーンに遷移
        SceneManager.LoadScene("Load");
    }


    IEnumerator LoadData()
    {
        // シーンの読み込みをする
        async = SceneManager.LoadSceneAsync(sceneName);


        Vector3 centerPos = center.transform.position;
        Vector3 pos = mooonObj.transform.position;


        while (!async.isDone)
        {


            float value = async.progress / 0.9f;

            pos.x = radius * (-Mathf.Cos(pi * value)) + centerPos.x;
            pos.y = radius * Mathf.Sin(pi * value) + centerPos.y;


            mooonObj.transform.position = pos;

            yield return null;
        }

    }


    IEnumerator WaitLoadData()
    {
        // シーンの読み込みをする
        async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;

        Vector3 pos = mooonObj.transform.position;
        Vector3 centerPos = center.transform.position;
        Vector3 childPos = mooonObj.transform.GetChild(1).localPosition;

        while (time < 5.0f)
        {
            float value = time / 5.0f;

            if (value < 0.01f)
            {
                value = 0.01f;
            }

            pos.x = radius * (-Mathf.Cos(pi * value)) + centerPos.x;
            pos.y = radius * Mathf.Sin(pi * value * 0.95f) + centerPos.y;

            childPos.x = -(96.0f * value);



            mooonObj.transform.position = pos;
            mooonObj.transform.GetChild(1).localPosition = childPos;



            yield return null;
        }

        async.allowSceneActivation = true;



    }


}
