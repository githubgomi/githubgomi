using System.Collections;
using System.Collections.Generic;
using EasyTransition;
using UnityEngine;

public class LoadScene : MonoBehaviour
{
    public TransitionSettings transition;
    public float loaddelay;

    // Start is called before the first frame update
    void Start()
    {
        TransLoadScene("Result_test");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TransLoadScene(string _scenename)
    {
        TransitionManager.Instance().Transition(_scenename,transition,loaddelay);
    }
}
