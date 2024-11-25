using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetCanvas1 : MonoBehaviour
{
    public Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {
        canvas.enabled = false; //スタート時非表示にする
    }

    // Update is called once per frame
    void Update()
    {
         if(Input.GetKeyDown(KeyCode.P)){ //Escapeボタンが押されたら
            canvas.enabled = !canvas.enabled; //非表示のCanvasを表示する
        }
    }
}
