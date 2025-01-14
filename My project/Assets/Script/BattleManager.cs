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
    public string Name = "RPGHeroHP"; // 名前
    public Transform SpawnPoint; // 生成する位置
    public Transform SecondSpawnPoint;
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

    //private Enemy enemy2;
    List<GameObject> II = new List<GameObject>(); //インスタンス化された敵の情報

    List<string> EnemyName = new List<string>();

    public static List<Player> players = new List<Player>(); //プレイヤーのplayerスクリプト
    private List<GameObject> PlayerObject = new List<GameObject>(); //プレイヤーのオブジェクトのほう
    public static List<Enemy> enemys = new List<Enemy>(); //エネミーのenemyスクリプト
    public List<object> AllCharacter = new List<object>();

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
    [SerializeField]private GameObject kariplayer;
    [SerializeField]private Player SecondPlayer;
    private List<Player> oomoto = new List<Player>();
    Animator anim;
    void Start()
    {
        oomoto.Add(player);
        oomoto.Add(SecondPlayer);

        //LoadAsset(assetAddress);
        //EnemyName.Add("Goblin");
        //EnemyName.Add("suraimu");
        EnemyName.Add("GruntHP");
        EnemyName.Add("SlimePBR");
        EnemyName.Add("TurtleShellPBR");

        SetupBattle();
        StartCoroutine(StartBattle());

        
        // プレイヤーを探してHPバーを生成
        //GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
        /*
        foreach(GameObject player in PlayerObject) //バンドルアセットを試してみたら非同期で遅かったため、場所を変更
        {
            Debug.Log(player.name);
            CreateHealthBarFor(player);
        } 
        */

        foreach(GameObject player in PlayerObject) //プレイヤーのhpバー
        {
            CreateHealthBarFor(player);
        } 

        // 敵を探してHPバーを生成（複数の敵に対応）
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            CreateHealthBarFor(enemy);
        }

        //ScriptPlayer.OnStatsUpdated += SyncPlayerStats;

    }

    void LoadAsset(string address)
    {
        // 非同期でアセットをロード
        Addressables.LoadAssetAsync<GameObject>(address).Completed += OnAssetLoaded;
    }

    void OnAssetLoaded(AsyncOperationHandle<GameObject> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject kari = Instantiate(obj.Result); // オブジェクトをインスタンス化
            PlayerObject.Add(kari);
            players.Add(kari.GetComponent<Player>());
        }
        else
        {
            Debug.LogError("アセットロード失敗");
        }
    }
    void UnloadAsset(GameObject loadedObject)
    {
        Addressables.ReleaseInstance(loadedObject);
    }
    public void GenerateSkillButtons(Player player)
    {
        //GameObject button = Instantiate(skillpanel)
        GameObject panelTransform = Instantiate(skillpanel,panerspawn);
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
            //playerComponent.healthBarManager = healthBarManager;
        }
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
        if(attackbotton.activeSelf == true)
        {
            attackbotton.SetActive(!attackbotton.activeSelf);
            escapebotton.SetActive(!escapebotton.activeSelf);
        }
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


        // ボタンが押されるまで待機
        actionSelected = false; // 初期化
        while (!actionSelected)
        {
            yield return null; // 1フレーム待機
        }

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

        // 敵の行動処理を実装
        int damage = Random.Range(enemy.AT - 5, enemy.AT + 5);
        damage -= (targetPlayer.armor[0].number + targetPlayer.defence);
        if (damage < 0) damage = 0;

        // 攻撃演出をここでいれたい
        anim = enemy.GetComponent<Animator>();
        StartCoroutine(PlayAttackAnimation());
        ClearBattleLog();
        string colorCode = ColorUtility.ToHtmlStringRGB(Color.red);
        battleLog.text +=  $"\n<color=#{colorCode}>{PlayerObject[playernumber].name}が{damage}のダメージを受けた!</color>";
        //battleLog.text += $"\n敵がプレイヤーに{damage}のダメージ！";
        Debug.Log($"{targetPlayer.name} に {damage} のダメージ！");
        targetPlayer.TakeDamage(damage);

        yield return new WaitForSeconds(2f); // デモ用の遅延
    }
    void OnActionSelected(string action)
    {
        Debug.Log($"選択されたアクション: {action}");
        actionSelected = true; // ボタンが押されたことを通知
    }
    public void OnActionSelected()
    {
        attackakuction = true; // ボタンが押されたことを通知
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
        PlayerObject.Add(Instance);
        ScriptPlayer = Instance.GetComponent<Player>();
        players.Add(ScriptPlayer);
        players[0].SetUpBattleManager(this);

        GameObject sp = Instantiate(kariplayer,SecondSpawnPoint.position,Quaternion.identity);
        PlayerObject.Add(sp);
        players.Add(sp.GetComponent<Player>());
        players[1].SetUpBattleManager(this);

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
                        GameObject kari = Instantiate(Inst, enemySpawnPoint0.position, Quaternion.identity);
                        enemys.Add(kari.GetComponent<Enemy>());
                    }else{
                        GameObject kari = Instantiate(Inst, enemySpawnPoint1.position, Quaternion.identity);
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
        StatusOver();
        BattleData.Instance.SetEnemyData( "", 0);

        // フィールドシーンに戻る
        SceneManager.LoadScene("SampleScene");
    }
    public void StatusOver()
    {
        foreach(Player player in players)
        {
            foreach(Player player2 in oomoto)
            {
                if(player.pn == player2.pn)
                {
                    player2.health = player.health;
                    player2.currentHealth = player.currentHealth;
                    player2.maxHealth = player.maxHealth;
                    player2.attack = player.attack;
                    player2.defence = player.defence;
                    player2.Speed = player.Speed;
                    player2.LV = player.LV;
                    player2.XP = player.XP;
                    player2.MaxXp = player.MaxXp;
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
