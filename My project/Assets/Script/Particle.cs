using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    private bool w;
    private bool a;
    private bool s;
    private bool d;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            w = true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            a = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            s = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            d = true;
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            w = false;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            a = false;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            s = false;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            d = false;
        }

        if (!w && !a && !s && !d)
        {
            Destroy(this.gameObject);
            chara.run = false;
        }
    }
}
