using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class scoreUI : MonoBehaviour
{

    [SerializeField] private Image[] UI = new Image[6];
    [SerializeField] private float[] Line = new float[6];


    [Space(15),Tooltip("Good����̕␳�l")][Range(-1.0f,1.0f)][SerializeField] private float Good_Correction_value;
    [Space(15),Tooltip("Miss����̕␳�l")][Range(-1.0f,1.0f)][SerializeField] private float Miss_Correction_value;

    private float MAX_Score = 1000000.0f;
    private static float score = 0.0f;
    private static float addScore = 0.0f;
    private Vector3 pos = new Vector3(1700.0f, 900.0f, 0.0f);
    private Vector3 N_pos = new Vector3(2500.0f, 950.0f, 0.0f);

    private bool isOnce = false;
    private static float Good;
    private static float Miss;


    // Start is called before the first frame update
    void Start()
    {
        Good = Good_Correction_value;
        Miss = Miss_Correction_value;

        Debug.Log($"���C��0{Line[5]}");
        Debug.Log($"pos0{UI[5].rectTransform.position}");
    }

    // Update is called once per frame
    void Update()
    {
        if(!isOnce)
        {
            addScore = MAX_Score / (LoadChartData.Data.Notes.Count / 2);    //�m�[�c�P�̍ō��_

            isOnce = true;
        }

        for (int i = 0; i < 6; ++i) 
        {
            if (score >= Line[i])
            {
                UI[i].rectTransform.position = pos;
                if(i<5)
                {
                    UI[i+1].rectTransform.position = N_pos;
                }
                break;
            }
        }

        //UI[3].rectTransform.position = new Vector3(1920.0f, 1080.0f, 0.0f);
    }

    public static void AddScore(RhythmDetect.Dodge dodge)
    {
        switch (dodge)
        {
        case RhythmDetect.Dodge.stylish:
            score += addScore;
            break;

         case RhythmDetect.Dodge.good:
            score += addScore * Good;
            break;

        case RhythmDetect.Dodge.miss:
            score += addScore * Miss;
            break;
      
        }
    }
}
