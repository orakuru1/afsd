using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureBox : MonoBehaviour
{
    private bool isPlayerNearby = false; //プレイヤーが近くにいるか
    private bool isTrejar = true; //宝箱が開けられてるかどうか
    public GameObject check; // 「Eキーで調べる」のUI
    [SerializeField]int golrd;
    [SerializeField]Player player;
    // Start is called before the first frame update
    void Start()
    {
        if (check != null)
        {
            check.SetActive(false); // 最初は非表示
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isPlayerNearby && Input.GetKeyDown(KeyCode.E) && isTrejar) //Eキーが押されたら
        {
            //宝箱が開くようなアニメーション
            player.GetGolrd(golrd);
            isTrejar = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // プレイヤーが範囲内に入った
        {
            isPlayerNearby = true;
    
            if (check != null)
            {
                check.SetActive(true); // UIを表示
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // プレイヤーが範囲外に出た
        {
            isPlayerNearby = false;
            if (check != null)
            {
                check.SetActive(false); // UIを非表示
            }
        }
    }
}
