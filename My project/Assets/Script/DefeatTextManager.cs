using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefeatTextManager : MonoBehaviour
{
    public GameObject defeatText;
    public GameObject player;
    private bool isTextActive = false;
    // Start is called before the first frame update
    void Start()
    {
        defeatText.SetActive(false);
    }

    //討伐完了テキストを表示
    public void ShowDefeatText()
    {
        defeatText.SetActive(true);
        isTextActive = true;

        //プレイヤの操作を無効か
        if(player != null)
        {
            player.GetComponent<chara>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //クリックでテキストを消してプレイヤーの操作を有効か
        if(isTextActive && Input.GetMouseButtonDown(0))
        {
            defeatText.SetActive(false);
            isTextActive = false;

            //プレイヤの操作を最有効化
            if(player != null)
            {
                player.GetComponent<chara>().enabled = true;
            }
        }
    }
}
