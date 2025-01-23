using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleData : MonoBehaviour
{
    public static BattleData Instance { get; private set; }

    //public GameObject enemyPrefab; // 戦闘シーンに持っていく敵のPrefab
    public string enemyName;       // 敵の名前などの情報
    public int enemyHealth;        // 敵の体力
    [SerializeField]private string playername;
    [SerializeField]private int health; //死んだ処理のHP
    [SerializeField]private float maxHealth;//一緒になってる
    [SerializeField]private int attack; //攻撃力
    [SerializeField]private int defence; //防御力
    [SerializeField]private int Speed;
    [SerializeField]private int LV; //現在のレベル
    [SerializeField]private double XP; //現在の経験値
    [SerializeField]private double MaxXp; //次のレベルアップの値
    [SerializeField]private float currentHealth; //HPバーに反映される値　(前のHPとごっちゃになった)
    public List<string> mainplayers = new List<string>();

    private void Awake()
    {
        // シングルトンパターンを実装
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーン遷移時に削除されないようにする
        }
        else
        {
            Destroy(gameObject); // すでに存在する場合は削除
        }
    }
    public void currentplayers(List<string> pn)
    {
        mainplayers.Clear();
        foreach(string str in pn)
        {
            mainplayers.Add(str);
        }
    }
    public void SetEnemyData(string name, int health)
    {
        enemyName = name;
        enemyHealth = health;
    }
    public void SetPlayerStatus(string pn,int health2,float maxHealth2,int attack2,int defence2,int Speed2,int LV2,double XP2,double MaxXp2,float currentHealth2)
    {
        playername = pn;
        health = health2;
        maxHealth = maxHealth2;
        attack = attack2;
        defence = defence2;
        Speed = Speed2;
        LV = LV2;
        XP = XP2;
        MaxXp = MaxXp2;
        currentHealth = currentHealth2;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
