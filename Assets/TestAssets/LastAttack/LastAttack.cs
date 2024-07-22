using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class LastAttack : MonoBehaviour
{
    // PlayableDirector‚ğ‚ÂTimeline‚ÌQÆ
    public GameObject timeline;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //I‚í‚éŠÔ‚æ‚ètimeline‚ªÄ¶‚³‚ê‚Ä‚¢‚½Às
        if (timeline.GetComponent<PlayableDirector>().duration - 0.1f <= timeline.GetComponent<PlayableDirector>().time)
        {
         
            S_Load.SetNextLoadScene("Result");
        }
    }
}
