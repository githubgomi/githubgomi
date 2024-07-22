using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody rb;
    float Speed = 2.0f;
    void Start()
    {

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーの移動（wasd）
        if (Input.GetKey(KeyCode.W))
        {
            rb.velocity = transform.forward * Speed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.velocity = -transform.right * Speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.velocity = -transform.forward * Speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.velocity = transform.right * Speed;
        }
        
    }
}
