using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public string[] dialogue; //NPCの会話内容
    private bool isPlayerNearby = false; //プレイヤーが近くにいるか
    public GameObject interactUI; // 「Eキーで話しかける」のUI
    private DialogueManager dialogueManager; //DialogueManagerへの参照
    [SerializeField] GameObject panel;

    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>(); // DialogueManagerを探す
        if (interactUI != null)
        {
            interactUI.SetActive(false); // 最初は非表示
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isPlayerNearby && Input.GetKeyDown(KeyCode.E)) //Eキーが押されたら
        {
            if (!dialogueManager.IsDialogueActive())
            {
                // 会話を開始
                dialogueManager.StartDialogue(dialogue);
            }
            else
            {
                // 次のテキストに進む
                dialogueManager.DisplayNextSentence();
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // プレイヤーが範囲内に入った
        {
            isPlayerNearby = true;
    
            if (interactUI != null)
            {
                interactUI.SetActive(true); // UIを表示
            }

            DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
            dialogueManager.ClearItemButtons(); // ボタンを削除
            dialogueManager.EndDialogue(); // 会話を終了
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // プレイヤーが範囲外に出た
        {
            isPlayerNearby = false;
            dialogueManager.EndDialogue();
            if (interactUI != null)
            {
                interactUI.SetActive(false); // UIを非表示
                panel.SetActive(false);
            }
        }
    }
}
