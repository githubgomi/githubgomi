using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//シーンマネージャー
using UnityEngine.SceneManagement;


public class S_SceneChange : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
    }

  
  　　　//シーンチェンジ
       public static void SetNextScene(int Index)
       {
     　int BuildIndex = SceneManager.GetActiveScene().buildIndex;

          //選択したステージ数分足す（ステージ1のIndexが0のため＋１）
           BuildIndex += Index + 1;
         //シーン読み込みを読み込む
         SceneManager.LoadScene(BuildIndex);
       }
 
}
