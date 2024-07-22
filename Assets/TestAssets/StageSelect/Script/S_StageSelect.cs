using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_StageSelect : MonoBehaviour
{
    // Start is called before the first frame update


    public float Speed;          //移動スピード
                                 //
    private int Index = 0;      //選択ステージ
    int MaxStage = 0;            //最大ステージ数
   

    //親オブジェクト
    GameObject StageObject;

    //子オブジェクト
    Transform[] StagePos;

    Vector3 TargetPos;   //ステージ毎の場所


    void Start()
    {
        //ステージの親オブジェクトを取得
        StageObject = GameObject.Find("StageObject");

        //子供分の配列を確保
        StagePos = new Transform[StageObject.transform.childCount];

        //子供のトランスフォームを持ってくる
        for(int i =0;i<StageObject.transform.childCount;i++)
        {
            StagePos[i] = StageObject.transform.GetChild(i);
        }

        //最大ステージ数
        MaxStage = StageObject.transform.childCount;

       

       TargetPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {       
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (Index < MaxStage -1)
            {
                Index++;
                //指定したステージの座標を持ってくる
                TargetPos.x = StagePos[Index].position.x;              
            } 
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
           if(Index > 0)
            {
                Index--;
                //指定したステージの座標を持ってくる
                TargetPos.x = StagePos[Index].position.x;
            }

        }

        //目標のＹとZ座標はいらないのでプレイヤーの座標で上書き
        TargetPos.y = transform.position.y;
        TargetPos.z = transform.position.z;

        //目標の座標に向かう
        transform.position = Vector3.MoveTowards(transform.position, TargetPos, Speed);

        if (Input.GetKeyDown(KeyCode.Return))
        {
            S_SceneChange.SetNextScene(Index);
        }

    }


  



}




