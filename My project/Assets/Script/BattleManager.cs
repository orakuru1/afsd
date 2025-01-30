using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class BattleManager : MonoBehaviour
{
    [SerializeField]private List<Transform> SpawnPoint = new List<Transform>();
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
    public GameObject hpBarPrefab; // HPバーのPrefab
    public Transform hpBarParent; // HPバーの親（Canvas）
    List<string> EnemyName = new List<string>();

    public static List<Player> players = new List<Player>(); //プレイヤーのplayerスクリプト
    private List<GameObject> PlayerObject = new List<GameObject>(); //プレイヤーのオブジェクトのほう
    public static List<Enemy> enemys = new List<Enemy>(); //エネミーのenemyスクリプト
    public List<object> AllCharacter = new List<object>();
    [SerializeField]private GameObject EnemyGuage; //敵のゲージ
    [SerializeField]private GameObject gaugebutton; //ゲージ技のボタン
    [SerializeField] private GameObject skillButtonPrefab; // 技ボタンのプレハブ
    [SerializeField] private GameObject skillpanel;
    [SerializeField] public Transform skillListParent; // ボタンを配置する親オブジェクト
    [SerializeField] public Transform panerspawn; // ボタンを配置する親オブジェクト
    private List<GameObject> insta = new List<GameObject>();
    [SerializeField] private GameObject attackbotton; // 技選択UIパネル
    [SerializeField] private GameObject escapebotton; // 技選択UIパネル
    private bool actionSelected = false;
    private bool attackakuction = false;
    public string assetAddress = "Assets/Resources_moved/otamesi.prefab";
    [SerializeField]private Player SecondPlayer;
    private List<Player> oomoto = new List<Player>(); //プレイヤーの大本のスクリプトプレイヤーが増えるたびに増やす。名前で今誰のがあるのか判断しよう
    [SerializeField]private GameObject GuageBar;
    private GameObject guagebutton;
    private string colorCode;
    private GameObject panelTransform;
    Animator anim;

    void Start()
    {
        colorCode = ColorUtility.ToHtmlStringRGB(Color.red);
        oomoto.Add(player);
        oomoto.Add(SecondPlayer);

        //EnemyName.Add("Goblin");
        //EnemyName.Add("suraimu");
        EnemyName.Add("GruntHP");
        EnemyName.Add("SlimePBR");
        EnemyName.Add("TurtleShellPBR");

        SetupBattle();
        StartCoroutine(StartBattle());

        foreach(GameObject player in PlayerObject) //プレイヤーのhpバー
        {
            CreateHealthBarFor(player);
            CreateGuageBar(player);
        } 

        // 敵を探してHPバーを生成（複数の敵に対応）
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); //findを使わないようにできるよ
        foreach (GameObject enemy in enemies)
        {
            CreateHealthBarFor(enemy);    //ゲージ技の生成もここでやる。次回予定
            GenerateEnemyGuageButton(enemy);
        }

        //ScriptPlayer.OnStatsUpdated += SyncPlayerStats;
        SetupPlayers();
    }


    public void GenerateSkillButtons(Player player)
    {
        //GameObject button = Instantiate(skillpanel)
        panelTransform = Instantiate(skillpanel,panerspawn);
        foreach (Skill skill in player.skills)
        {
            GameObject button = Instantiate(skillButtonPrefab, panelTransform.transform);
            insta.Add(button);
            button.GetComponentInChildren<Text>().text = skill.skillName;

            // ボタンが押されたときにスキルを実行
            Button btn = button.GetComponent<Button>();
            btn.onClick.AddListener(() => player.Attack(skill,player,panelTransform));
            btn.onClick.AddListener(() => OnActionSelected(skill.skillName));
        }
    }
    public void GenerateGuageButtons(Player player) //ゲージ技のボタン
    {
        guagebutton = Instantiate(gaugebutton,panerspawn);
        player.SetUpSPN(guagebutton);
        //guagebutton.GetComponentInChildren<Text>().text =  ;

        Button btn = guagebutton.GetComponent<Button>();
        btn.onClick.AddListener(() => player.OnSpecialAction(player));
    }
    public void BuckSpecial() //スペシャルボタンを押す前に非表示にする
    {
        guagebutton.SetActive(!guagebutton.activeSelf);
        panelTransform.SetActive(!panelTransform.activeSelf);
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

        Player playerComponent = character.GetComponent<Player>();
        if (playerComponent != null)
        {
            healthBarManager.player = playerComponent;
        }
    }
    void CreateGuageBar(GameObject character)
    {
        GameObject guagebar = Instantiate(GuageBar,hpBarParent);

        GaugeManager gaugemanager = character.AddComponent<GaugeManager>();
        gaugemanager.gaugeInstance = guagebar;

        Player playerComponent = character.GetComponent<Player>();
        if (playerComponent != null)
        {
            gaugemanager.player = playerComponent;
        }
    }
    public void GenerateEnemyGuageButton(GameObject character)
    {
        GameObject gameObject = Instantiate(EnemyGuage,hpBarParent);

        EnemyDestroyGuage enemyguage = character.AddComponent<EnemyDestroyGuage>();
        enemyguage.SetObject(gameObject);

    }

    public void ClearBattleLog()
    {
        battleLog.text = ""; // 空文字列でテキストをクリア
    }

    public void AddLog(string message)
    {
        battleLog.text += $"\n{message}"; // メッセージを追加
    }

    private IEnumerator StartBattle()
    {
        battleLog.text = "戦闘開始！";
        if(attackbotton.activeSelf == true)
        {
            attackbotton.SetActive(!attackbotton.activeSelf);
            escapebotton.SetActive(!escapebotton.activeSelf);
        }
        yield return new WaitForSeconds(1f);
        UpdateBattleState();
    }

    void UpdateBattleState()  //全然機能してないから消してもいい
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
                    Debug.Log($" {player.name} のターン");
                    yield return PlayerTurn(player);
                }
                else if (character is Enemy enemy && enemy.health > 0) // 生存している場合
                {
                    Debug.Log($" {enemy.name} のターン");
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
        GaugeManager gaugeManager = player.GetComponent<GaugeManager>();
        gaugeManager.FillGauge(10f);
        battleLog.text = $"{player.name} のターン！";

        if(attackbotton.activeSelf == false)
        {
            attackbotton.SetActive(!attackbotton.activeSelf);
            escapebotton.SetActive(!escapebotton.activeSelf);
        }
        attackakuction = false;
        while (!attackakuction)
        {
            yield return null;
        }
        GenerateSkillButtons(player);
        GenerateGuageButtons(player);

        // ボタンが押されるまで待機
        actionSelected = false; // 初期化
        while (!actionSelected)
        {
            yield return null; // 1フレーム待機
        }
        Destroy(guagebutton);

        yield return new WaitForSeconds(2f); // デモ用の遅延
    }

    IEnumerator EnemyTurn(Enemy enemy)
    {
        battleLog.text = $"{enemy.name} のターン！";
        yield return new WaitForSeconds(2f);

            int playernumber;
            Player targetPlayer;
        do
        {
            playernumber = Random.Range(0,players.Count);
            targetPlayer = players[playernumber];
        }
        while(targetPlayer.health <= 0);


        if(attackbotton.activeSelf == true)
        {
            attackbotton.SetActive(!attackbotton.activeSelf);
            escapebotton.SetActive(!escapebotton.activeSelf);
        }
        if(enemy.IsTurn() != false) //敵のゲージがバーストしていないか
        {
            battleLog.text +=  $"\n<color=#{colorCode}>{enemy.name}は動けない！</color>";
            yield return new WaitForSeconds(2f); // 遅延
            enemy.GetComponent<EnemyDestroyGuage>().SetGuage();
        }
        else
        {
            // 敵の行動処理を実装
            int damage = Random.Range(enemy.AT - 5, enemy.AT + 5);
            damage -= (targetPlayer.armor[0].number + targetPlayer.defence);
            if (damage < 0) damage = 0;

            // 攻撃演出をここでいれたい
            anim = enemy.GetComponent<Animator>();
            StartCoroutine(PlayAttackAnimation());
            ClearBattleLog();
            battleLog.text +=  $"\n<color=#{colorCode}>{PlayerObject[playernumber].name}が{damage}のダメージを受けた!</color>";
            //battleLog.text += $"\n敵がプレイヤーに{damage}のダメージ！";
            Debug.Log($"{targetPlayer.name} に {damage} のダメージ！");
            targetPlayer.TakeDamage(damage);
            yield return new WaitForSeconds(2f); // 遅延
        }


    }
    void OnActionSelected(string action)
    {
        Debug.Log($"選択されたアクション: {action}");
        actionSelected = true; // ボタンが押されたことを通知
    }
    public void OnActionSelected()
    {
        attackakuction = true; // ボタンが押されたことを通知
        if(attackbotton.activeSelf == true)
        {
            attackbotton.SetActive(!attackbotton.activeSelf);
            escapebotton.SetActive(!escapebotton.activeSelf);
        }
    }

    bool IsBattleOver()
    {
        // バトル終了条件
        bool allPlayersDead = players.All(p => p.health <= 0);
        bool allEnemiesDead = enemys.All(e => e.health <= 0);

        return allPlayersDead || allEnemiesDead;
    }

    IEnumerator PlayAttackAnimation()
    {
        // 攻撃アニメーションやエフェクトを実行
        anim.SetBool("attack", true);
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("attack", false);
        //Debug.Log($"{enemy.name} の攻撃アニメーションが再生されます！");
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
        if(BattleData.Instance.mainplayers[0] != null)
        {
            for(int i = 0 ; i < BattleData.Instance.mainplayers.Count ; i++)
            {
                GameObject playerprefab = (GameObject)Resources.Load(BattleData.Instance.mainplayers[i]);
                Instance = Instantiate(playerprefab, SpawnPoint[i].position, Quaternion.identity);
                PlayerObject.Add(Instance);
                ScriptPlayer = Instance.GetComponent<Player>();
                players.Add(ScriptPlayer);
                ScriptPlayer.SetUpBattleManager(this);
            }
        }

        // BattleDataから敵の情報を取得して設定
        if (BattleData.Instance.enemyName != null)
        {
            // 敵のPrefabを生成
            GameObject prefab = (GameObject)Resources.Load (BattleData.Instance.enemyName);
            GameObject hab = Instantiate(prefab, enemySpawnPoint.position, enemySpawnPoint.rotation);
            //hab.transform.rotation = Quaternion.Euler(0, 180, 0);
            enemys.Add(hab.GetComponent<Enemy>());
            int EnemyCount = Random.Range(0,3);
            if(EnemyCount != 0)
            {
                for(int i = 0; i < EnemyCount; i++)
                {
                    int index = (int)Random.Range(0.0f,EnemyName.Count); //敵の名前を習得できる数字がランダムで作られる
                    GameObject Inst = (GameObject)Resources.Load(EnemyName[index]);
                    if(i != 0)
                    {
                        GameObject kari = Instantiate(Inst, enemySpawnPoint0.position, enemySpawnPoint0.rotation);
                        enemys.Add(kari.GetComponent<Enemy>());
                    }else{
                        GameObject kari = Instantiate(Inst, enemySpawnPoint1.position, enemySpawnPoint1.rotation);
                        enemys.Add(kari.GetComponent<Enemy>());
                    }
                }
            }

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
        StatusOver();
        BattleData.Instance.SetEnemyData( "", 0);

        // フィールドシーンに戻る
        SceneManager.LoadScene("GAMEMAPP");
    }



    private void ShowPlayerAndGameOver()
    {
        // "GAME OVER" テキストを表示
        gameOverText.gameObject.SetActive(true);
    }
    void SetupPlayers()
    {
        foreach(Player player in players)
        {
            if(player.pn == "RPGHeroHP")
            {
                player.SetSpecialSkill(player.gameObject.AddComponent<AOESpecial>());
            }
            else if(player.pn == "otamesi")
            {
                player.SetSpecialSkill(player.gameObject.AddComponent<HealAllSpecial>());
            }
        }
    }
    public void StatusOver()
    {
        foreach(Player player in players)
        {
            foreach(Player player2 in oomoto)
            {
                if(player.pn == player2.pn)
                {
                    Debug.Log(player.pn);
                    player2.health = player.health;
                    player2.currentHealth = player.currentHealth;
                    player2.maxHealth = player.maxHealth;
                    player2.attack = player.attack;
                    player2.defence = player.defence;
                    player2.Speed = player.Speed;
                    player2.LV = player.LV;
                    player2.XP = player.XP;
                    player2.MaxXp = player.MaxXp;
                    player2.gold = player.gold;
                    player2.currentGauge = player.currentGauge;
                }
            }
        }
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
