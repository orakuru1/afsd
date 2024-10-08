using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCharact : MonoBehaviour
{
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); //rigidbodyを取得
    }

    // Update is called once per frame
    void Update()
    {
        //wキー(前方移動)
        if(Input.GetKey(KeyCode.W))
        {
            Vector3 normal = new Vector3(0f,0f,1f);
            rb.AddForce(normal);
        }
        // Sキー（後方移動）
        if (Input.GetKey(KeyCode.S))
        {
            Vector3 normal = new Vector3(0f,0f,-1f);
            rb.AddForce(normal);
        }
 
        // Dキー（右移動）
        if (Input.GetKey(KeyCode.D))
        {
            Vector3 normal = new Vector3(1f,0f,0f);
            rb.AddForce(normal);
        }
 
        // Aキー（左移動）
        if (Input.GetKey(KeyCode.A))
        {
            Vector3 normal = new Vector3(-1f,0f,0f);
            rb.AddForce(normal);
        }
    }
}
