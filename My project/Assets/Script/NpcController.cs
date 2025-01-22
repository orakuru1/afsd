using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NpcController : MonoBehaviour
{
    [SerializeField]private Text ChatLog;
    private bool isPlayerInRange;
    private bool isTalking = false;    // 会話中かどうかのフラグ
    
    void Start()
    {
        ChatLog.gameObject.SetActive(false); // 非表示
    }

    // Update is called once per frame
    void Update()
    {
        // 範囲内にプレイヤーがいるとき、左クリックを検出
        if (isPlayerInRange && Input.GetMouseButtonDown(0) &&!isTalking)
        {
            StartCoroutine(OnInteract());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // トリガーエリアからプレイヤーが出たとき
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            ChatLog.gameObject.SetActive(false); // 非表示
        }
    }

    private IEnumerator OnInteract()
    {
        // クリックされたときの処理
        isTalking = true;
        ChatLog.gameObject.SetActive(true);
        ChatLog.text = "この先は危険だ！！";
        yield return new WaitForSeconds(2);
        ChatLog.text = "敵がたくさんおるんじゃ";
        yield return new WaitForSeconds(2);
        ChatLog.text = "行くっていうなら別に止めはせん";
        yield return new WaitForSeconds(2);
        ChatLog.text = "だがこれだけは言っておく";
        yield return new WaitForSeconds(2);
        ChatLog.text = "死ぬなよ！！";
        for(int i = 0; i <=5; i++)
        {
            ChatLog.text += "ぁ";
            yield return new WaitForSeconds(1);
        }
        isTalking = false;    // 会話中フラグをオフ
                
        //ChatLog.text = ""; //ログをクリアできる
    }
}
