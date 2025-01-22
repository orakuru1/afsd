using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text dialogueText; //テキスト表示用のUI
    public GameObject dialogueBox; //テキストボックスのUI
    private Queue<string> sentences; //会話内容をキューで管理
    private bool dialogueActive = false; //会話中かどうか

    public GameObject itemButtonPrefab; // アイテム用のボタンPrefab
    public Transform itemButtonParent; // ボタンの親（Scroll View内）
    public Text currentEquipmentText; // 現在の装備を表示するUI
    [SerializeField]private EquipmentManager equipmentManager;
    [SerializeField] GameObject shoppanel;
    private List<GameObject> itemButtons = new List<GameObject>(); // 生成されたボタンを管理するリスト

    void Start()
    {
        sentences = new Queue<string>();
        dialogueBox.SetActive(false); // 最初は非表示
        shoppanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartDialogue(string[] dialogue)
    {
        dialogueActive = true;
        dialogueBox.SetActive(true);
        sentences.Clear();

        foreach (string sentence in dialogue)
        {
            sentences.Enqueue(sentence); // 会話内容をキューに追加
        }

        DisplayNextSentence();
    }
    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {

            shoppanel.SetActive(true);

            //古いボタンを削除
            ClearItemButtons();

            // ショップアイテムをUIに反映
            foreach (var item in equipmentManager.WeaponItems)
            {
                CreateItemButton(item);
            }
            foreach (var item in equipmentManager.ArmorItems)
            {
                CreateItemButton(item);
            }

            // 現在の装備を更新
            UpdateCurrentEquipmentUI();

            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        dialogueText.text = sentence; // テキストを更新
    }

    public void EndDialogue()
    {
        dialogueActive = false;
        dialogueBox.SetActive(false); // テキストボックスを非表示
    }

    public bool IsDialogueActive()
    {
        return dialogueActive; // 会話中かどうかを返す
    }

    void CreateItemButton(Weapon item)
    {
        // ボタンを生成
        GameObject button = Instantiate(itemButtonPrefab, itemButtonParent);
        itemButtons.Add(button);

        Text buttonText = button.GetComponentInChildren<Text>();

        // ボタンのテキストを設定
        buttonText.text = $"{item.equipName} - {item.price}ゴールド";

        // ボタンのクリックイベントを設定
        Button btn = button.GetComponent<Button>();
        btn.onClick.AddListener(() => BuyItem(item));
    }
    void CreateItemButton(Armor item)
    {
        // ボタンを生成
        GameObject button = Instantiate(itemButtonPrefab, itemButtonParent);
        itemButtons.Add(button);

        Text buttonText = button.GetComponentInChildren<Text>();

        // ボタンのテキストを設定
        buttonText.text = $"{item.armorname} - {item.price}ゴールド";

        // ボタンのクリックイベントを設定
        Button btn = button.GetComponent<Button>();
        btn.onClick.AddListener(() => BuyItem(item));
    }
    void BuyItem(Weapon item)
    {
        Player player = FindObjectOfType<Player>();
        // ゴールドが足りるか確認
        if (ChangeCharacter.ScriptPlayers[0].SpendGold(item.price)) //大本のスクリプトの値が変わるようにしたけど、インスタンス化されたプレイヤーの値が変わったほうがいいのか、わからない
        {
            // 購入処理
            equipmentManager.EquipItem(item);
            Debug.Log($"{item.equipName} を購入しました！");

            // UIを更新
            UpdateCurrentEquipmentUI();
        }
        else
        {
            Debug.Log("購入に失敗しました。");
        }
    }
    void BuyItem(Armor item)
    {
        Player player = FindObjectOfType<Player>();
        // ゴールドが足りるか確認
        if (ChangeCharacter.ScriptPlayers[0].SpendGold(item.price)) //大本のスクリプトの値が変わるようにしたけど、インスタンス化されたプレイヤーの値が変わったほうがいいのか、わからない
        {
            // 購入処理
            equipmentManager.EquipItem(item);
            Debug.Log($"{item.armorname} を購入しました！");

            // UIを更新
            UpdateCurrentEquipmentUI();
        }
        else
        {
            Debug.Log("購入に失敗しました。");
        }
    }

    void UpdateCurrentEquipmentUI()
    {
        // 現在の装備を表示
        //currentEquipmentText.text = $"現在の装備: {equipmentManager.currentEquipment.equipName}\n説明: {equipmentManager.currentEquipment.description}";
    }
    public void ClearItemButtons()
    {
        // 生成されたボタンをすべて削除
        foreach (GameObject button in itemButtons)
        {
            Destroy(button);
        }

        // リストをクリア
        itemButtons.Clear();
    }
}
