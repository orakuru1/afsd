using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFalse : MonoBehaviour
{
    public Canvas canvas;  //変数
    // Start is called before the first frame update
    void Start()
    {
        canvas.enabled = false; //スタート時非表示にする
    }

    // Update is called once per frame
    void Update()
    {

    }
}
