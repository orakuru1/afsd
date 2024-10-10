using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCharact : MonoBehaviour
{
    Rigidbody rb;
    bool MovuFlagForward = false;
    bool MovuFlagRight = false;
    bool MovuFlagLeft = false;
    bool MovuFlagBack = false;

    // Start is called before the first frame update
    void Start()
    {
        // フレームレートを400psに設定する
        Application.targetFrameRate = 400;
        rb = GetComponent<Rigidbody>(); //rigidbodyを取得
    }

    void FixedUpdate()
    {
        //wキー(前方移動)
        if(MovuFlagForward)
        {
            Vector3 normal = new Vector3(0f,0f,7f);
            rb.AddForce(normal);
            MovuFlagForward = false;
        }
        // Sキー（後方移動）
        if(MovuFlagBack)
        {
            Vector3 normal = new Vector3(0f,0f,-7f);
            rb.AddForce(normal);
            MovuFlagBack = false;
        }
        // Dキー（右移動）
        if(MovuFlagRight)
        {
            Vector3 normal = new Vector3(7f,0f,0f);
            rb.AddForce(normal);
            MovuFlagRight = false;
        }
        // Aキー（左移動）
        if(MovuFlagLeft)
        {
            Vector3 normal = new Vector3(-7f,0f,0f);
            rb.AddForce(normal);
            MovuFlagLeft = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        //wキー(前方移動)
        if(Input.GetKey(KeyCode.W))
        {
            MovuFlagForward = true;
        }
        // Sキー（後方移動）
        if (Input.GetKey(KeyCode.S))
        {
            MovuFlagBack = true;
        }
 
        // Dキー（右移動）
        if (Input.GetKey(KeyCode.D))
        {
            MovuFlagRight = true;
        }
 
        // Aキー（左移動）
        if (Input.GetKey(KeyCode.A))
        {
            MovuFlagLeft = true;
        }
    }
}
