using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class InventoryManager : MonoBehaviour
{
    [SerializeField]private GameObject InventoBotton; //生成するインベントリのボタン
    [SerializeField]private Transform Inventoparent; //インベントリボタンの親
    [SerializeField]private GameObject InventoScrol; //インベントリのスクロール
    [SerializeField]private GameObject descriptionText; //アイテムの説明テキスト
    [SerializeField]private Text descriptionText2; //テキストを変えるところ
    [SerializeField]private GameObject gearButton; //装備を確認するボタン
    [SerializeField]private GameObject InventoAll;
    [SerializeField]private GameObject BuckButton;
    private List<GameObject> Buttons = new List<GameObject>();
    void Start()
    {
        descriptionText.SetActive(!descriptionText.activeSelf);
        InventoScrol.SetActive(!InventoScrol.activeSelf);
        InventoAll.SetActive(!InventoAll.activeSelf);
        BuckButton.SetActive(!BuckButton.activeSelf);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            InventoAll.SetActive(!InventoAll.activeSelf);
            //InventoScrol.SetActive(!InventoScrol.activeSelf);
            //descriptionText.SetActive(!descriptionText.activeSelf);
            //descriptionText2.text = "";
            //CrieitoWeapon();
            //CrieitoArmor();
        }
    }

    public void OnbuttonBuck()
    {
        //InventoAll.SetActive(!InventoAll.activeSelf);
        InventoScrol.SetActive(!InventoScrol.activeSelf);
        descriptionText.SetActive(!descriptionText.activeSelf);
        BuckButton.SetActive(!BuckButton.activeSelf);
    }

    public void OnbuttonInventoWeapon() //戻るボタンとインベントリの枠以外の場所を押したら、非表示にするようにする。何回も押したら、ボタンが無限に増えてしまうので、一個前の奴を全部消す処理を作ろうかな。
    {
        BuckButton.SetActive(!BuckButton.activeSelf);
        InventoAll.SetActive(!InventoAll.activeSelf);
        InventoScrol.SetActive(!InventoScrol.activeSelf);
        descriptionText.SetActive(!descriptionText.activeSelf);
        descriptionText2.text = "";
        CrieitoWeapon();
    }

    public void OnbuttonInventoArmor()
    {
        BuckButton.SetActive(!BuckButton.activeSelf);
        InventoAll.SetActive(!InventoAll.activeSelf);
        InventoScrol.SetActive(!InventoScrol.activeSelf);
        descriptionText.SetActive(!descriptionText.activeSelf);
        descriptionText2.text = "";
        CrieitoArmor();
    }
    private void DestroyButton()
    {
        foreach(GameObject go in Buttons)
        {
            Destroy(go);
        }
    }
    private void HighlightButton(GameObject button)
    {
        Outline outline = button.AddComponent<Outline>();
        outline.effectColor = Color.yellow; // アウトラインの色を黄色に設定
        outline.effectDistance = new Vector2(5, 5); // アウトラインの太さ
    }
    private void CrieitoWeapon()
    {
        int i = 0;
        DestroyButton();
        foreach(Weapon weapon in ChangeCharacter.ScriptPlayers[0].weapon)
        {
            GameObject botton = Instantiate(InventoBotton,Inventoparent);
            Buttons.Add(botton); //ここのリストは分けたほうがいいかも。
            Text bottontext = botton.GetComponentInChildren<Text>();

            bottontext.text = weapon.equipName;

            Image buttonimage = botton.GetComponent<Image>();
            if(weapon.BuckSprite != null)
            {
                buttonimage.sprite = weapon.BuckSprite;
            }

            Button btn = botton.GetComponent<Button>();
            btn.onClick.AddListener(() => geardescription(weapon));

            if(i == 0)
            {
                HighlightButton(botton);
            }
        }
    }
    private void CrieitoArmor()
    {
        int i = 0;
        DestroyButton();
        foreach(Armor armor in ChangeCharacter.ScriptPlayers[0].armor)
        {
            GameObject botton = Instantiate(InventoBotton,Inventoparent);
            Buttons.Add(botton);
            Text bottontext = botton.GetComponentInChildren<Text>();

            bottontext.text = armor.armorname;

            Image buttonimage = botton.GetComponent<Image>();
            if(armor.BuckSprite != null)
            {
                buttonimage.sprite = armor.BuckSprite;
            }

            Button btn = botton.GetComponent<Button>();
            btn.onClick.AddListener(() => geardescription(armor));

            if(i == 0)
            {
                HighlightButton(botton);
            }
        }
    }
    private void geardescription(Weapon TentativeWeapon)
    {
        if(descriptionText.activeSelf == false) descriptionText.SetActive(!descriptionText.activeSelf);
        descriptionText2.text = TentativeWeapon.description;
        ChangeCharacter.ScriptPlayers[0].weapon.Remove(TentativeWeapon); //押された奴と同じ奴の中から、１個消える
        ChangeCharacter.ScriptPlayers[0].weapon.Insert(0, TentativeWeapon); //０番目に挿入して、それまでの奴は一個後ろに移動する
        CrieitoWeapon();
    }
    private void geardescription(Armor TentativeArmor)
    {
        if(descriptionText.activeSelf == false) descriptionText.SetActive(!descriptionText.activeSelf);
        descriptionText2.text = TentativeArmor.description;
        ChangeCharacter.ScriptPlayers[0].armor.Remove(TentativeArmor); //押された奴と同じ奴の中から、１個消える
        ChangeCharacter.ScriptPlayers[0].armor.Insert(0, TentativeArmor); //０番目に挿入して、それまでの奴は一個後ろに移動する
        CrieitoArmor();
    }
}
