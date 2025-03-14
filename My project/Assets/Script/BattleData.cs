using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    [SerializeField]private Image FadeImage;

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

    public void SetImage(Image image)
    {
        FadeImage = image;
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
        
        Vector3 respawnPosition; //リスポーン位置を記録する変数

        GameObject p = GameObject.Find("Player");
        
        respawnPosition = p.transform.position;
        PlayerPrefs.SetFloat("RespawnX", respawnPosition.x);
        PlayerPrefs.SetFloat("RespawnY", respawnPosition.y);
        PlayerPrefs.SetFloat("RespawnZ", respawnPosition.z);
        PlayerPrefs.Save();

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

        asyncLoad = SceneManager.LoadSceneAsync("GAMEMAPP");
        
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

    public IEnumerator FadeInBuluck(float duration)
    {
        float StartTime = Time.time;
        while(Time.time < StartTime + duration)
        {
            float alpha = Mathf.Lerp(0, 1, (Time.time - StartTime) / duration);
            FadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        FadeImage.color = new Color(0, 0, 0, 1); // 最終的に完全に暗くする
    }

    public IEnumerator FadeOutBluck(float duration)
    {
        FadeImage.color = new Color(0, 0, 0, 1);//最初に暗くする

        float StartTime = Time.time;
        while(Time.time < StartTime + duration)
        {
            float alpha = Mathf.Lerp(1, 0, (Time.time - StartTime) / duration);
            FadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        FadeImage.color = new Color(0, 0, 0, 0); // 完全に見えるようにする
    }

    public float GetElementalMultiplier(Element attack, Element defender)
    {
        if(attack == Element.Fire && defender == Element.Grass) return 1.2f;
        if(attack == Element.Water && defender == Element.Fire) return 1.2f;
        if(attack == Element.Grass && defender == Element.Water) return 1.2f;

        if(attack == Element.Fire && defender == Element.Water) return 0.8f;
        if(attack == Element.Water && defender == Element.Grass) return 0.8f;
        if(attack == Element.Grass && defender == Element.Fire) return 0.8f;

        return 1f;
    }

    public void GetCurentDamage(Dictionary<Element, float> damageD, Dictionary<Element, float> DurationD, Element Type, float Damage, float duratin, float probability)
    {
        if(Type == Element.None) return;

        float ran = Random.Range(0,100); //0～99
        if(probability <= ran) return;
        
    }

    public bool IsCurentDamage(float probability)
    {
        float ran = Random.Range(0,100); //0～99
        if(probability <= ran) return false;

        return true;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
