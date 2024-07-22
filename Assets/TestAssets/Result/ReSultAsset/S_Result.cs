using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.UI;
public class S_Result : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stylishText;
    [SerializeField] private TextMeshProUGUI normalText;
    [SerializeField] private TextMeshProUGUI badText;

    static private float stylishCnt;
    static private float normalCnt;
    static private float badCnt;

    [SerializeField] private Image stylishImage;
    [SerializeField] private Image otherImage;

    [SerializeField] private Sprite[] rankSprite;

    System.Tuple<float, float, float> count;

    static public void SetScore(float stylish ,float normal,float bad)
    {
        stylishCnt = stylish;
        normalCnt = normal;
        badCnt = bad;
    }


    // Start is called before the first frame update
    void Start()
    {
        

        stylishText.SetText(stylishCnt.ToString());
        normalText.SetText(normalCnt.ToString());
        badText.SetText(badCnt.ToString());

        float totalCnt = normalCnt + badCnt+ stylishCnt;

        if(totalCnt == 0)
            totalCnt = 1;


        int rank =  (int)(stylishCnt / totalCnt * rankSprite.Length) -1;

        if(rank == rankSprite.Length - 1)
        {
            stylishImage.sprite = rankSprite[rank];
            Destroy(otherImage);
            return;
        }


        otherImage.sprite = rankSprite[rank]; 
        Destroy(stylishImage);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
