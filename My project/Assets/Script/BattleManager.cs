using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

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
    public static List<Enemy> enemys = new List<Enemy>();
    public List<object> AllCharacter = new List<object>();

    [SerializeField] private GameObject skillButtonPrefab; // 技ボタンのプレハブ
    [SerializeField] private GameObject skillpanel;
    [SerializeField] public Transform skillListParent; // ボタンを配置する親オブジェクト
    [SerializeField] public Transform panerspawn; // ボタンを配置する親オブジェクト
    public static GameObject panelTransform;
    private List<GameObject> insta = new List<GameObject>();
    [SerializeField] private GameObject attackbotton; // 技選択UIパネル
    [SerializeField] private GameObject escapebotton; // 技選択UIパネル
    private bool actionSelected = false;
    Animator anim;
    void Start()
    {
        
        //EnemyName.Add("Goblin");
        //EnemyName.Add("suraimu");
        EnemyName.Add("GruntHP");
        EnemyName.Add("SlimePBR");
        EnemyName.Add("TurtleShellPBR");

        SetupBattle();
        StartCoroutine(StartBattle());

        
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
            btn.onClick.AddListener(() => OnActionSelected(skill.skillName));

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

    private IEnumerator StartBattle()
    {
        battleLog.text = "戦闘開始！";
        yield return new WaitForSeconds(1f);
        UpdateBattleState();
    }

    void UpdateBattleState()
    {
        if (players.All(p => p.health <= 0))
        {
            EndBattle2(false); // プレイヤーの敗北
        }
        else if (enemys.All(e => e.health <= 0))
        {
            EndBattle2(true); // プレイヤーの勝利
        }
        else
        {
            StartCoroutine(BattleLoop());
        }
    }
    void OtameshiUpdate()
    {
        AllCharacter.Clear();
        // 全プレイヤーと敵をリストに追加
        AllCharacter.AddRange(players);
        AllCharacter.AddRange(enemys);

        // スピードで降順にソート
        AllCharacter = AllCharacter.OrderByDescending(character =>
        {
            if (character is Player player)
                return player.Speed;
            if (character is Enemy enemy)
                return enemy.Speed;
            return 0;
        }).ToList();
    }
    IEnumerator BattleLoop()
    {
        while (true)
        {
            // スピード順に行動
            foreach (var character in AllCharacter.ToList()) // コピーを作成してループ
            {
                if (character is Player player && player.health > 0) // 生存している場合
                {
                    Debug.Log($"プレイヤー {player.name} のターン");
                    yield return PlayerTurn(player);
                }
                else if (character is Enemy enemy && enemy.health > 0) // 生存している場合
                {
                    Debug.Log($"敵 {enemy.name} のターン");
                    yield return EnemyTurn(enemy);
                }

                // バトル終了条件を確認
                if (IsBattleOver())
                {
                    StartCoroutine(EndBattleSequence());
                    yield break;
                }
            }

            // ソートし直して次のターンへ
            OtameshiUpdate();
        }
    }
    public void hyouzi(int damage)
    {
        battleLog.text += $"\nプレイヤーが敵に{damage}のダメージ！";
    }
    IEnumerator PlayerTurn(Player player)
    {
        if(attackbotton.activeSelf == false)
        {
            attackbotton.SetActive(!attackbotton.activeSelf);
            escapebotton.SetActive(!escapebotton.activeSelf);
        }
        battleLog.text = $"{player.name} のターン！";

        // ボタンが押されるまで待機
        actionSelected = false; // 初期化
        while (!actionSelected)
        {
            yield return null; // 1フレーム待機
        }

        Debug.Log($"{player.name} のアクションが終了しました");
        yield return new WaitForSeconds(2f); // デモ用の遅延
    }

    IEnumerator EnemyTurn(Enemy enemy)
    {
        if(attackbotton.activeSelf == true)
        {
            attackbotton.SetActive(!attackbotton.activeSelf);
            escapebotton.SetActive(!escapebotton.activeSelf);
        }
        battleLog.text = $"{enemy.name} のターン！";

        // 敵の行動処理を実装
        int damage = Random.Range(enemy.AT - 5, enemy.AT + 5);
        damage -= (players[0].armor[0].number + players[0].defence);
        if (damage < 0) damage = 0;

        // 攻撃演出をここでいれたい
        anim = enemy.GetComponent<Animator>();
        StartCoroutine(PlayAttackAnimation());
        ClearBattleLog();
        string colorCode = ColorUtility.ToHtmlStringRGB(Color.red);
        battleLog.text +=  $"\n<color=#{colorCode}>プレイヤーが{damage}のダメージを受けた!</color>";
        //battleLog.text += $"\n敵がプレイヤーに{damage}のダメージ！";
        players[0].TakeDamage(damage);

        yield return new WaitForSeconds(2f); // デモ用の遅延
    }
    void OnActionSelected(string action)
    {
        Debug.Log($"選択されたアクション: {action}");
        actionSelected = true; // ボタンが押されたことを通知
    }

    bool IsBattleOver()
    {
        // バトル終了条件
        bool allPlayersDead = players.All(p => p.health <= 0);
        bool allEnemiesDead = enemys.All(e => e.health <= 0);

        return allPlayersDead || allEnemiesDead;
    }
/*
    public void PlayerAttack(int damage)
    {
        if (!isPlayerTurn) return;

        //int damage = Random.Range(5, 15);
        ClearBattleLog();

        //enemy.TakeDamage(damage);

        isPlayerTurn = false;
        UpdateBattleState();

        enemy2 = FindObjectOfType<Enemy>();  ///違うやつが探されそう
        if (enemy != null)
        {
            battleManager = FindObjectOfType<BattleManager>();
            battleManager.PlayerAttack();
            //enemy.TakeDamage(damage);
        }

    }

    IEnumerator EnemyTurn()        //敵のターン
    {
        yield return new WaitForSeconds(0.2f); //0.2秒待機

        // EnemyManager.enemies をループして攻撃処理を実行
        for (int i = 0;i < EnemyManager.enemies.Count; i++)
        {
            Enemy enemy = EnemyManager.enemies[i];
            if (enemy != null) // 敵が有効な場合のみ攻撃
            {
                yield return new WaitForSeconds(3); //3秒待機
                int damage = Random.Range(enemy.AT-5, enemy.AT+5);
                // 攻撃演出をここでいれたい
                anim = enemy.GetComponent<Animator>();
                StartCoroutine(PlayAttackAnimation());
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
    */

    IEnumerator PlayAttackAnimation()
    {
        // 攻撃アニメーションやエフェクトを実行
        anim.SetBool("attack", true);
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("attack", false);
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
            GameObject hab = Instantiate(prefab, enemySpawnPoint.position, enemySpawnPoint.rotation);
            //hab.transform.rotation = Quaternion.Euler(0, 180, 0);
            enemys.Add(hab.GetComponent<Enemy>());
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
                        GameObject kari = Instantiate(Inst, enemySpawnPoint0.position, enemySpawnPoint.rotation);
                        enemys.Add(kari.GetComponent<Enemy>());
                    }else{
                        GameObject kari = Instantiate(Inst, enemySpawnPoint1.position, enemySpawnPoint.rotation);
                        enemys.Add(kari.GetComponent<Enemy>());
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
