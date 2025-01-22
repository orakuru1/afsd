using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public List<Weapon> WeaponItems = new List<Weapon>(); // ショップのアイテムリスト
    public List<Armor> ArmorItems = new List<Armor>();
    public Weapon currentEquipment; // 現在の装備

    void Start() //購入したアイテムをプレイヤーが持ってるリストに反映させる。できれば、今まで持ってる装備とかも一覧して持てて、選べるようにしたい。(インベントリ機能の作成するときが来たかも)お金の反映もする。他のところでも、違う商品のショップを作れるかやる。
    {
        // ショップの初期データを設定
        WeaponItems.Add(new Weapon { equipName = "木の剣", number = 10, description = "初心者向けの剣", price = 50});
        WeaponItems.Add(new Weapon { equipName = "鉄の剣", number = 30, description = "鋼鉄の剣で威力が上がる", price = 100});
        ArmorItems.Add(new Armor { armorname = "魔法の盾", number = 20, description = "防御力が向上する盾", price = 100});

        // 初期装備を設定
        //currentEquipment = player.equips[0]; // 現在持っている武器
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void EquipItem(Weapon item)
    {
        //currentEquipment = item; // 装備を更新
        ChangeCharacter.ScriptPlayers[0].weapon.Add(item);
        Debug.Log($"新しい装備: {item.equipName}");
    }
    public void EquipItem(Armor item)
    {
        //currentEquipment = item; // 装備を更新
        ChangeCharacter.ScriptPlayers[0].armor.Add(item);
        Debug.Log($"新しい装備: {item.armorname}");
    }
    public void ArmorItem(Armor item)
    {
        ChangeCharacter.ScriptPlayers[0].armor.Add(item);
    }
}
