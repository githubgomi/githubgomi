using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�V�[���}�l�[�W���[
using UnityEngine.SceneManagement;


public class S_SceneChange : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
    }

  
  �@�@�@//�V�[���`�F���W
       public static void SetNextScene(int Index)
       {
     �@int BuildIndex = SceneManager.GetActiveScene().buildIndex;

          //�I�������X�e�[�W���������i�X�e�[�W1��Index��0�̂��߁{�P�j
           BuildIndex += Index + 1;
         //�V�[���ǂݍ��݂�ǂݍ���
         SceneManager.LoadScene(BuildIndex);
       }
 
}
