using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class BattleManager : MonoBehaviour
{
    public string Name = "RPGHeroHP"; // 名前
    public Transform SpawnPoint; // 生成する位置
    private GameObject Instance;
    //↑キャラクター生成用
    public TextMeshProUGUI gameOverText; // 対象のTextコンポーネント
    public Image fadeImage; // フェード用のImage

    public Transform enemySpawnPoint; // 敵を生成する位置
    public Transform enemySpawnPoint0; // 敵を生成する位置
    public Transform enemySpawnPoint1; // 敵を生成する位置
    public Text enemyNameText;        // 敵の名前を表示するUI
    public Slider enemyHealthSlider;  // 敵の体力を表示するUI

    public Text battleLog;           // バトルログを表示するUI
    [SerializeField]private Player player;            // プレイヤーのスクリプト
    [SerializeField]private Player ScriptPlayer;      //プレイヤー自体のスクリプト //これを参照してEXPを送るようにすれば何とかなるかも、倒したときの処理
    public Enemy enemy;              // 敵のスクリプト
    private bool isPlayerTurn = true; // プレイヤーのターンかどうか

    public GameObject hpBarPrefab; // HPバーのPrefab
    public Transform hpBarParent; // HPバーの親（Canvas）

    //private Enemy enemy2;
    List<GameObject> II = new List<GameObject>(); //インスタンス化された敵の情報

    List<string> EnemyName = new List<string>();

    public static List<Player> players = new List<Player>();

    [SerializeField] private GameObject skillButtonPrefab; // 技ボタンのプレハブ
    [SerializeField] private GameObject skillpanel;
    [SerializeField] public Transform skillListParent; // ボタンを配置する親オブジェクト
    [SerializeField] public Transform panerspawn; // ボタンを配置する親オブジェクト
    public static GameObject panelTransform;
    private List<GameObject> insta = new List<GameObject>();
    [SerializeField] private GameObject attackbotton; // 技選択UIパネル
    [SerializeField] private GameObject escapebotton; // 技選択UIパネル
    
    void Start()
    {

        EnemyName.Add("Goblin");
        EnemyName.Add("suraimu");

        SetupBattle();
        StartBattle();

        // プレイヤーを探してHPバーを生成
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            CreateHealthBarFor(player);
        }
        else{
            Debug.Log("いません！");
        }

        // 敵を探してHPバーを生成（複数の敵に対応）
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            CreateHealthBarFor(enemy);
        }

        ScriptPlayer.OnStatsUpdated += SyncPlayerStats;

    }

    public void GenerateSkillButtons()
    {
        panelTransform = Instantiate(skillpanel,panerspawn);
        //GameObject button = Instantiate(skillpanel)
        foreach (Skill skill in players[0].skills)
        {
            GameObject button = Instantiate(skillButtonPrefab, panelTransform.transform);
            insta.Add(button);
            Debug.Log(button);  //ボタンに新しいスクリプトを入れてここで、スキルのダメージを受け取れるようにする。ボタンのスクリプトでplayerattackを呼び出す
            button.GetComponentInChildren<Text>().text = skill.skillName;

            // ボタンが押されたときにスキルを実行
            Button btn = button.GetComponent<Button>();
            btn.onClick.AddListener(() => players[0].Attack(skill));

        }
    }

    // フェードアウト（画面が暗くなる）
    public IEnumerator FadeOut(float duration)
    {
        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            float alpha = Mathf.Lerp(0, 1, (Time.time - startTime) / duration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        fadeImage.color = new Color(0, 0, 0, 1); // 最終的に完全に暗くする
        Debug.Log("暗くなった？");
    }

    void SyncPlayerStats()
    {
        player.health = ScriptPlayer.health;
        player.maxHealth = ScriptPlayer.maxHealth;
        player.currentHealth = ScriptPlayer.currentHealth;
        player.attack = ScriptPlayer.attack;
        player.defence = ScriptPlayer.defence;
        player.LV = ScriptPlayer.LV;
        player.XP = ScriptPlayer.XP;
        player.MaxXp = ScriptPlayer.MaxXp;
        
        if(player.LV == 2 && player.XP == 0)
        {
            player.skills = ScriptPlayer.skills;
            player.skills.Add(new Skill { skillName = "ice", damage = 20, description = "A ball of fire that burns enemies." });
        }
        

        // 必要に応じて他の値も同期
    }

    void CreateHealthBarFor(GameObject character)
    {
        // HPバーを生成して親に設定
        GameObject hpBar = Instantiate(hpBarPrefab, hpBarParent);

        // キャラクターの位置に応じたHPバーを管理するスクリプトを設定
        HealthBarManager healthBarManager = character.AddComponent<HealthBarManager>();
        healthBarManager.hpBarInstance = hpBar;
    }

    public void ClearBattleLog()
    {
        battleLog.text = ""; // 空文字列でテキストをクリア
    }

    public void AddLog(string message , Color color)
    {
        battleLog.text += $"\n<color=#{ColorUtility.ToHtmlStringRGB(color)}>{message}</color>"; // メッセージを追加
    }

    void StartBattle()
    {
        battleLog.text = "戦闘開始！";
        isPlayerTurn = true;
        UpdateBattleState();
    }

    void UpdateBattleState()
    {
        if (players[0].health <= 0)
        {
            EndBattle2(false); // プレイヤーの敗北
        }
        else if (enemy.health <= 0)
        {
            EndBattle2(true); // プレイヤーの勝利
        }
        else
        {
            if (isPlayerTurn)
            {
                battleLog.text += "\nプレイヤーのターン！";
                EnablePlayerActions(true);
            }
            else
            {
                battleLog.text += "\n敵のターン！";
                EnablePlayerActions(false);
                StartCoroutine(EnemyTurn()); // コルーチンとして実行
            }
        }
    }

    public void PlayerAttack(int damage)
    {
        if (!isPlayerTurn) return;

        //int damage = Random.Range(5, 15);
        ClearBattleLog();
        battleLog.text += $"\nプレイヤーが敵に{damage}のダメージ！";
        //enemy.TakeDamage(damage);

        isPlayerTurn = false;
        UpdateBattleState();
/*
        enemy2 = FindObjectOfType<Enemy>();  ///違うやつが探されそう
        if (enemy != null)
        {
            battleManager = FindObjectOfType<BattleManager>();
            battleManager.PlayerAttack();
            //enemy.TakeDamage(damage);
        }
*/
    }

    IEnumerator EnemyTurn()        //敵のターン
    {
        // EnemyManager.enemies をループして攻撃処理を実行
        for(int i = 0;i < EnemyManager.enemies.Count; i++)
        {
            Enemy enemy = EnemyManager.enemies[i];
            if (enemy != null) // 敵が有効な場合のみ攻撃
            {
                yield return new WaitForSeconds(2f); //2秒待機
                int damage = Random.Range(enemy.AT-5, enemy.AT+5);
                // 攻撃演出をここでいれたい
                PlayAttackAnimation(enemy);
                ClearBattleLog();
                string colorCode = ColorUtility.ToHtmlStringRGB(Color.red);
                battleLog.text +=  $"\n<color=#{colorCode}>プレイヤーが{damage}のダメージを受けた!</color>";
                //AddLog($"\n敵がプレイヤーに{damage}のダメージ！",Color.red);
                players[0].TakeDamage(damage); // プレイヤーにダメージを与える
            }
        }
        isPlayerTurn = true;
        attackbotton.SetActive(!attackbotton.activeSelf);
        escapebotton.SetActive(!escapebotton.activeSelf);
        //敵のターンが終わった時にボタンが復活するようにする......................................................................
        UpdateBattleState();
    }

    void PlayAttackAnimation(Enemy enemy)
    {
        // 攻撃アニメーションやエフェクトを実行
        //Debug.Log($"{enemy.name} の攻撃アニメーションが再生されます！");
    }

    void EnablePlayerActions(bool enable)
    {
        // プレイヤーの行動UI（ボタンなど）を有効化または無効化
        // ここでは例として簡単に設定

        //Button attackButton = GameObject.Find("AttackButton").GetComponent<Button>();
        //attackButton.interactable = enable;
        
    }

    void EndBattle2(bool isPlayerWin)
    {
        if (isPlayerWin)
        {
            battleLog.text += "\nプレイヤーの勝利！";
        }
        else
        {
            StartCoroutine(EndBattleSequence());
            battleLog.text += "\nプレイヤーの敗北...";
        }
    }

    void SetupBattle()  
    {
        GameObject playerprefab = (GameObject)Resources.Load (Name);
        Instance = Instantiate(playerprefab, SpawnPoint.position, Quaternion.identity);
        ScriptPlayer = Instance.GetComponent<Player>();
        players.Add(ScriptPlayer);
        // BattleDataから敵の情報を取得して設定
        if (BattleData.Instance.enemyName != null)
        {
            // 敵のPrefabを生成
            GameObject prefab = (GameObject)Resources.Load (BattleData.Instance.enemyName);
            II.Add(Instantiate(prefab, enemySpawnPoint.position, Quaternion.identity));
            int EnemyCount = Random.Range(0,3);
            Debug.Log(EnemyCount);
            if(EnemyCount != 0)
            {
                for(int i = 0; i < EnemyCount; i++)
                {
                    int index = (int)Random.Range(0.0f,EnemyName.Count); //敵の名前を習得できる数字がランダムで作られる
                    Debug.Log(index);
                    GameObject Inst = (GameObject)Resources.Load(EnemyName[index]);
                    if(i != 0)
                    {
                        II.Add(Instantiate(Inst, enemySpawnPoint0.position, Quaternion.identity));
                    }else{
                        II.Add(Instantiate(Inst, enemySpawnPoint1.position, Quaternion.identity));
                    }
                }
            }

            
            //II.Add(Instantiate(prefab, enemySpawnPoint1.position, Quaternion.identity));


            //Debug.Log(BattleData.Instance.enemyPrefab);

            /*
            // UIに敵の情報を反映
            enemyNameText.text = BattleData.Instance.enemyName;
            enemyHealthSlider.maxValue = BattleData.Instance.enemyHealth;
            enemyHealthSlider.value = BattleData.Instance.enemyHealth;
            */
        }
    }

    public void EndBattle()
    {
        // BattleDataの情報をリセット（必要に応じて）
        BattleData.Instance.SetEnemyData( "", 0);

        // フィールドシーンに戻る
        SceneManager.LoadScene("SampleScene");
    }

    private IEnumerator EndBattleSequence()
    {
        // フェードアウト
        yield return FadeOut(2.0f); // 2秒かけてフェードアウト

        // プレイヤーを表示する処理
        ShowPlayerAndGameOver();

        // 3秒待機
        yield return new WaitForSeconds(3.0f);

        // BattleDataの情報をリセット（必要に応じて）
        //BattleData.Instance.SetEnemyData("", 0);

        // フィールドシーンに戻る
        //SceneManager.LoadScene("SampleScene");
    }

    private void ShowPlayerAndGameOver()
    {
        // "GAME OVER" テキストを表示
        gameOverText.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
