using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GAMESCENE : MonoBehaviour
{
    private bool firstPush = false; //スタート
    private bool retrunPush = false; //戻る
    // Start is called before the first frame update
    //スタートボタンを押されたら呼ばれる
    public void PressStart()
    {
        Debug.Log("Press Start!");

        if(!firstPush){
            Debug.Log("Go Next Scene!");
            //次のシーンへ行く命令
            SceneManager.LoadScene("GAMEMAPP");
            firstPush = true;
        }
    }

     public void PressRetrun() //ゲーム画面からスタート画面へ
    {
        Debug.Log("Press Retrun");

        if(!retrunPush){
            Debug.Log("Next Scene!");

            SceneManager.LoadScene("title");
            retrunPush = true;
        }
    }


   

        
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       /* if(!retrunPush && Input.GetKey(KeyCode.S)){
            Debug.Log("Next Scene!");
            SceneManager.LoadScene("title");
            retrunPush = true;
        }*/    
    }
}
