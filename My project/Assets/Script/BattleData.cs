using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BattleData : MonoBehaviour
{
    public static BattleData Instance { get; private set; }
    [SerializeField]private int attack; //攻撃力
    [SerializeField]private int defence; //防御力
    [SerializeField]private int Speed;
    [SerializeField]private int LV; //現在のレベル
    [SerializeField]private int health; //死んだ処理のHP
    public int enemyHealth;        // 敵の体力

    public List<string> mainplayers = new List<string>();
    public string enemyName;       // 敵の名前などの情報
    [SerializeField]private string playername;

    [SerializeField]private double XP; //現在の経験値
    [SerializeField]private double MaxXp; //次のレベルアップの値

    [SerializeField]private float maxHealth;//一緒になってる
    [SerializeField]private float currentHealth; //HPバーに反映される値　(前のHPとごっちゃになった)

    private bool isplayer = true;

    [SerializeField]public Vector3 spawnposition = new Vector3(); //キャラクターが返って来るときの位置

    private AsyncOperation asyncLoad;

    private void Awake()
    {
        // シングルトンパターンを実装
        if (Instance == null)
        {
            Instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject); // シーン遷移時に削除されないようにする
        }
        else
        {
            Destroy(gameObject); // すでに存在する場合は削除
        }
    }
    public void RePosition(Vector3 vector3)
    {
        spawnposition = vector3;
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
    public bool isconhurikut()
    {
        return isplayer;
    }
    public void modorusyori()
    {
        isplayer = true;
    }

    public IEnumerator LoadBattleScene() //非同期処理で読み込みシーン移動
    {//**********************************本当にこれで最適化できてるか不安だからみる＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
        isplayer = false;

        asyncLoad = SceneManager.LoadSceneAsync("BattleScene");
        
        asyncLoad.allowSceneActivation = false; // シーンの切り替えを一時停止

        while (!asyncLoad.isDone) 
        {
            // 90% 以上読み込まれたら切り替え
            if (asyncLoad.progress >= 0.9f)//0.9で止まる。falseにしてると発動しない。trueで１になる
            {
                Resources.UnloadUnusedAssets();

                yield return new WaitForSeconds(1.5f);
                asyncLoad.allowSceneActivation = true; // シーン遷移を許可

            }
            yield return null;
        }

        asyncLoad = null;
    }
    
    public IEnumerator LoadMap()
    {
        asyncLoad = SceneManager.LoadSceneAsync("SampleScene");
        
        asyncLoad.allowSceneActivation = false; // シーンの切り替えを一時停止

        while (!asyncLoad.isDone)
        {
            // 90% 以上読み込まれたら切り替え
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true; // シーン遷移を許可
            }
            yield return null;
        }
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
