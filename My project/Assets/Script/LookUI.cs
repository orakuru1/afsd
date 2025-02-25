using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookUI : MonoBehaviour
{
    private Camera maincamera;

    void Start()
    {
        maincamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(maincamera != null)
        {
            transform.LookAt(maincamera.transform);
            transform.Rotate(0, 180, 0); // 逆向きにならないように調整
        }
    }
}
