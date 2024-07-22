using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_TargetCamera : MonoBehaviour
{
    //ターゲットする対象物
    public Transform target;
    //ターゲットまでのカメラの距離
    public Vector3 offset;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + offset;
    }
}
