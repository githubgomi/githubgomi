using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  //�V�[���J�ڂ��g���Ƃ��ɋL��

public class ChangeScene : MonoBehaviour
{
    [Space(10),Tooltip("�Ăт����V�[�����L��")]public string NextScene;   //unity���ŐG���悤�ɐ錾
      public void Change_Button()
    {
        SceneManager.LoadScene(NextScene);  //�V�[���؂�ւ�
    }
}
