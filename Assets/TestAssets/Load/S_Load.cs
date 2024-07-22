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
    [SerializeField] private GameObject center;//������钆�S�_
    [SerializeField] private GameObject mooonObj;
    [SerializeField] private float radius;//���S�_���猎�̉摜�܂ł̔��a

    private float pi = 3.14159265f;
    //�@�񓯊�����Ŏg�p����AsyncOperation
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

        //�t�@�C������
        // �P�M�K�o�C�g�ǂݍ��ނ̂ɑ�̂P�b���炢�Ɖ���
        //�������V�[��������t�H���_�̂Ƃ���Ƀp�X��ݒ�B�����̃t�H���_�̖��O������
        var fileInfo = new FileInfo(Application.dataPath + "/" + "ProductAssets/Scenes/" + name + ".unity");

        //���[�h���Ԃ�����
        loadTime = (fileInfo.Length / (1024f * 1024f * 1024));

        sceneName = Path.GetFileNameWithoutExtension(fileInfo.Name);

        //���ɓǂݍ��ރV�[����I�������烍�[�h�V�[���ɑJ��
        SceneManager.LoadScene("Load");
    }


    IEnumerator LoadData()
    {
        // �V�[���̓ǂݍ��݂�����
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
        // �V�[���̓ǂݍ��݂�����
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
