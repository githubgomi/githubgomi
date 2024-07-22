using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class LastAttack : MonoBehaviour
{
    // PlayableDirectorを持つTimelineの参照
    public GameObject timeline;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //終わる時間よりtimelineが再生されていた時実行
        if (timeline.GetComponent<PlayableDirector>().duration - 0.1f <= timeline.GetComponent<PlayableDirector>().time)
        {
         
            S_Load.SetNextLoadScene("Result");
        }
    }
}
