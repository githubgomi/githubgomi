using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_TargetCamera : MonoBehaviour
{
    //�^�[�Q�b�g����Ώە�
    public Transform target;
    //�^�[�Q�b�g�܂ł̃J�����̋���
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
