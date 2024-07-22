using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_StylishTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float life_Time;  //UIÇ™è¡Ç¶ÇÈÇ‹Ç≈ÇÃéûä‘


    private float nowTime;
    void Start()
    {
        nowTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        nowTime += Time.deltaTime;

        if(life_Time < nowTime)
        {
            Destroy(gameObject);
        }
    }
}
