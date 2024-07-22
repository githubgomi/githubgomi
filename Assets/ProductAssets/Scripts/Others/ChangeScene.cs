using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  //シーン遷移を使うときに記載

public class ChangeScene : MonoBehaviour
{
    [Space(10),Tooltip("呼びたいシーンを記入")]public string NextScene;   //unity側で触れるように宣言
      public void Change_Button()
    {
        SceneManager.LoadScene(NextScene);  //シーン切り替え
    }
}
