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

    public static List<Enemy> enemys = new List<Enemy>(); //エネミーのenemyスクリプト
    public static List<Player> players = new List<Player>(); //プレイヤーのplayerスクリプト
    public static Player LastAttackPlayer;
    public List<object> AllCharacter = new List<object>();

    public Transform enemySpawnPoint; // 敵を生成する位置
    public Transform enemySpawnPoint0; // 敵を生成する位置
    public Transform enemySpawnPoint1; // 敵を生成する位置
    public Transform hpBarParent; // HPバーの親（Canvas）
    [SerializeField]private Transform panerspawn; // ボタンを配置する親オブジェクト

    public Text battleLog;           // バトルログを表示するUI
    [SerializeField]private Text SulidoText; //誰かのターンテキスト

    public GameObject hpBarPrefab; // HPバーのPrefab

    public TextMeshProUGUI gameOverText; // 対象のTextコンポーネント

    public Image fadeImage; // フェード用のImage
    
    [SerializeField]private List<Transform> SpawnPoint = new List<Transform>();

    private List<GameObject> PlayerObject = new List<GameObject>(); //プレイヤーのオブジェクトのほう
    private List<GameObject> insta = new List<GameObject>();
    private List<GameObject> Uis = new List<GameObject>(); //HPやゲージなどのUI
    private List<GameObject> PlayerUis = new List<GameObject>(); //PlayerのHPやゲージなどのUI
    [SerializeField]private List<GameObject> LvLog = new List<GameObject>();
    private GameObject Instance;    //キャラクター生成用
    private GameObject guagebutton;
    private GameObject panelTransform;
    [SerializeField]private GameObject BattleLog;
    [SerializeField]private GameObject EnemyGuage; //敵のゲージ
    [SerializeField]private GameObject gaugebutton; //ゲージ技のボタン
    [SerializeField] private GameObject skillButtonPrefab; // 技ボタンのプレハブ
    [SerializeField] private GameObject skillpanel;
    [SerializeField] private GameObject attackbotton; // 技選択UIパネル
    [SerializeField] private GameObject escapebotton; // 技選択UIパネル
    [SerializeField]private GameObject GuageBar; //味方のゲージ
    [SerializeField]private GameObject LevelupLog;
    [SerializeField]private GameObject LevelupPanel;
    [SerializeField]private GameObject transparentButton; //透明なボタン
    [SerializeField]private GameObject BrackGraund;//暗くなる画像
    [SerializeField]private GameObject EnemyTarget;//ターゲットUI

    [SerializeField]private List<Player> oomoto = new List<Player>(); //プレイヤーの大本のスクリプトプレイヤーが増えるたびに増やす。名前で今誰のがあるのか判断しよう
    [SerializeField]private Player ScriptPlayer;      //プレイヤー自体のスクリプト //これを参照してEXPを送るようにすれば何とかなるかも、倒したときの処理

    private bool actionSelected = false;
    private bool attackakuction = false;
    public bool stayturn = true;
    public bool LebelupStay = false;

    private string colorCode;
    private string blueColor;
    private List<string> EnemyName = new List<string>();
    
    private Animator anim;
    [SerializeField]private Animator SlidoAnimation;

    private CameraMove cameraMove;
    
    void Start()
    {
        BattleData.Instance.SetImage(fadeImage);
        StartCoroutine(BattleData.Instance.FadeOutBluck(1.5f));

        transparentButton.SetActive(false);
        LevelupPanel.SetActive(false);

        saisyonohyouzi(); //邪魔だからオフにしておく

        cameraMove = Camera.main.GetComponent<CameraMove>(); // メインカメラのスクリプトを取得

        colorCode = ColorUtility.ToHtmlStringRGB(Color.red);//カラーの色を設定
        blueColor = ColorUtility.ToHtmlStringRGB(Color.blue);//青色

        //EnemyName.Add("Goblin");
        //EnemyName.Add("suraimu");
        EnemyName.Add("GruntHP");
        EnemyName.Add("SlimePBR");
        EnemyName.Add("TurtleShellPBR");

        SetupBattle();
        StartCoroutine(StartBattle());

        foreach(GameObject player in PlayerObject) //プレイヤーのhpバー
        {
            CreateGraund(player);
            CreateHealthBarFor(player);
            CreateGuageBar(player);
        } 

        // 敵を探してHPバーを生成（複数の敵に対応）
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); //findを使わないようにできるよ
        foreach (GameObject enemy in enemies)
        {
            CreateGraund(enemy);
            CreateHealthBarFor(enemy);    //ゲージ技の生成もここでやる。次回予定
            GenerateEnemyGuageButton(enemy);
        }

        //ScriptPlayer.OnStatsUpdated += SyncPlayerStats;
        CharaUifalse();
        SetupPlayers();
    }

    public void setlog()
    {
        EnemyTarget.SetActive(!EnemyTarget.activeSelf);
        BattleLog.SetActive(!BattleLog.activeSelf);
    }
    public void saisyonohyouzi()//最初の画面を見やすくするため
    {
        EnemyTarget.SetActive(!EnemyTarget.activeSelf);
        BattleLog.SetActive(!BattleLog.activeSelf);
        attackbotton.SetActive(!attackbotton.activeSelf);
        escapebotton.SetActive(!escapebotton.activeSelf);
    }

    public void LevelUpButton()//レベルアップ時にターンを止めて非表示
    {
        LebelupStay = !LebelupStay;
        transparentButton.SetActive(!transparentButton.activeSelf);
        LevelupPanel.SetActive(!LevelupPanel.activeSelf);
    }

    public void CharaUifalse()//敵味方のUIを非表示
    {
        foreach(GameObject Ui in Uis)
        {
            Ui.SetActive(false);
        }
    }

    public void CharaUitrue()//敵味方のUIを表示
    {
        foreach(GameObject Ui in Uis)
        {
            Ui.SetActive(true);
        }
    }

    public void PlayerUIFalse()
    {
        foreach(GameObject Ui in PlayerUis)
        {
            Ui.SetActive(false);
        }
    }

    public void PlayerUITrue()
    {
        foreach(GameObject Ui in PlayerUis)
        {
            Ui.SetActive(true);
        }
    }

    public void GenerateSkillButtons(Player player)//プレイヤーのスキルを生成
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

        player.GetComponent<GaugeManager>().GBASet(guagebutton.GetComponent<Animator>());
        //guagebutton.GetComponentInChildren<Text>().text =  ;

        Button btn = guagebutton.GetComponent<Button>();
        btn.onClick.AddListener(() => player.OnSpecialAction(player));
    }

    public void BuckSpecial() //スペシャルボタンを押す前に非表示にする
    {
        guagebutton.SetActive(!guagebutton.activeSelf);
        attackbotton.SetActive(!attackbotton.activeSelf);
        escapebotton.SetActive(!escapebotton.activeSelf);
    }

    public IEnumerator FadeOut(float duration)    // フェードアウト（画面が暗くなる）
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

    void CreateGraund(GameObject character) //HPばーなどの背景
    {
        GameObject bg = Instantiate(BrackGraund, hpBarParent);
        Uis.Add(bg);

        BGController bgc = character.AddComponent<BGController>();
        Text ElementText = bg.GetComponentInChildren<Text>();

        bgc.BG = bg;
        Player playerComponent = character.GetComponent<Player>();
        Enemy enemyComponent = character.GetComponent<Enemy>();

        if (playerComponent != null)
        {
            bgc.player = playerComponent;
            ElementText.text = ConvertToVertical(playerComponent.Race);
            PlayerUis.Add(bg);
        }
        else
        {
            ElementText.text = ConvertToVertical(enemyComponent.Race);
            bg.AddComponent<LookUI>();
        }

    }

    string ConvertToVertical(string input)
    {
        return string.Join("\n", input.ToCharArray()); // 文字ごとに改行を追加
    }

    void CreateHealthBarFor(GameObject character)//HPばーを生成
    {
        // HPバーを生成して親に設定
        GameObject hpBar = Instantiate(hpBarPrefab, hpBarParent);
        Uis.Add(hpBar);

        // キャラクターの位置に応じたHPバーを管理するスクリプトを設定
        HealthBarManager healthBarManager = character.AddComponent<HealthBarManager>();
        healthBarManager.hpBarInstance = hpBar;

        Player playerComponent = character.GetComponent<Player>();
        if (playerComponent != null)
        {
            healthBarManager.player = playerComponent;
            PlayerUis.Add(hpBar);
        }
        else
        {
            hpBar.AddComponent<LookUI>();
        }
    }

    void CreateGuageBar(GameObject character)//スペシャル技のゲージを生成
    {
        GameObject guagebar = Instantiate(GuageBar,hpBarParent);
        Uis.Add(guagebar);

        GaugeManager gaugemanager = character.AddComponent<GaugeManager>();
        gaugemanager.gaugeInstance = guagebar;

        Player playerComponent = character.GetComponent<Player>();
        if (playerComponent != null)
        {
            gaugemanager.player = playerComponent;
            PlayerUis.Add(guagebar);
        }
    }

    public void GenerateEnemyGuageButton(GameObject character)//敵のブレイク値を生成
    {
        GameObject gameObject = Instantiate(EnemyGuage,hpBarParent);
        Uis.Add(gameObject);

        EnemyDestroyGuage enemyguage = character.AddComponent<EnemyDestroyGuage>();
        enemyguage.SetObject(gameObject);

        gameObject.AddComponent<LookUI>();
    }

    public void ClearBattleLog() // 空文字列でテキストをクリア
    {
        battleLog.text = ""; 
    }

    public void AddLog(string message) // メッセージを追加
    {
        battleLog.text += $"\n{message}";
    }

    private IEnumerator StartBattle() //ここを経由する必要全然ないかも
    {
        battleLog.text = "戦闘開始！";
        if(attackbotton.activeSelf == true)
        {
            attackbotton.SetActive(!attackbotton.activeSelf);
            escapebotton.SetActive(!escapebotton.activeSelf);
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(BattleLoop());
    }

    void OtameshiUpdate() //プレイヤーとエネミーを全部集めてソート
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

    IEnumerator BattleLoop()//メインループ
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
                    player.UpdateBuffs(); // バフを更新
                }
                else if (character is Enemy enemy && enemy.health > 0) // 生存している場合
                {
                    SlidoAnimation.SetTrigger("NewTurn");
                    Debug.Log($" {enemy.name} のターン");
                    yield return EnemyTurn(enemy);
                    yield return enemy.ContinueCheck();
                    //＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝ここに継続ダメージを食らう処理
                }

                // バトル終了条件を確認
                if (IsBattleOver())
                {
                    yield break;
                }
            }

            // ソートし直して次のターンへ
            OtameshiUpdate();
        }
    }

    public void hyouzi(float damage) //上におんなじのがあったわ
    {
        battleLog.text += $"\nプレイヤーが敵に{damage}のダメージ！";
    }

    IEnumerator PlayerTurn(Player player)
    {
        while(stayturn == true || LebelupStay == true)//待機中
        {
            yield return null;
        }

        SulidoText.text = $"<color=#{blueColor}>{player.pn} のターン！</color>";
        yield return new WaitForSeconds(0.35f);

        SlidoAnimation.SetBool("TurnBool", true);
        PlayerUITrue();

        yield return new WaitForSeconds(0.5f);

        SlidoAnimation.SetBool("TurnBool", false);

        GaugeManager gaugeManager = player.GetComponent<GaugeManager>();
        gaugeManager.FillGauge(10f); //ゲージを増やす
        battleLog.text = $"{player.name} のターン！";

        if(attackbotton.activeSelf == false)
        {
            attackbotton.SetActive(!attackbotton.activeSelf);
            escapebotton.SetActive(!escapebotton.activeSelf);
        }
        GenerateGuageButtons(player);
        StartCoroutine(gaugeManager.Animation());

        attackakuction = false;

        while (!attackakuction)
        {
            yield return null;
        }

        StopCoroutine(gaugeManager.Animation());
        GenerateSkillButtons(player);

        // ボタンが押されるまで待機
        actionSelected = false; // 初期化

        while (!actionSelected)
        {
            yield return null; // 1フレーム待機
        }

        Destroy(guagebutton);

        yield return new WaitForSeconds(2f);

        //cameraMove.SetUp(enemySpawnPoint);
        //cameraMove.ComeBuckCamera();
    }

    IEnumerator EnemyTurn(Enemy enemy)//敵のターン
    {
        while(stayturn == true || LebelupStay == true)
        {
            yield return null;
        }

        SulidoText.text = $"<color=#{colorCode}>{enemy.en} のターン！</color>";
        yield return new WaitForSeconds(0.4f);

        SlidoAnimation.SetBool("TurnBool", true);
        PlayerUITrue();

        yield return new WaitForSeconds(0.5f);

        SlidoAnimation.SetBool("TurnBool", false);

        battleLog.text = $"{enemy.name} のターン！";
        

        yield return new WaitForSeconds(2f);

            int playernumber;
            Player targetPlayer;
        do
        {
            playernumber = Random.Range(0,players.Count);
            targetPlayer = players[playernumber];
        }
        while(targetPlayer.health <= 0);//HPが０の味方には攻撃しない

        if(attackbotton.activeSelf == true)
        {
            attackbotton.SetActive(!attackbotton.activeSelf);
            escapebotton.SetActive(!escapebotton.activeSelf);
        }

        if(enemy.isBurst == true) //敵のゲージがバーストしていないか
        {
            battleLog.text +=  $"\n<color=#{colorCode}>{enemy.name}は動けない！</color>";
            yield return new WaitForSeconds(2f); // 遅延
            enemy.GetComponent<EnemyDestroyGuage>().SetGuage();
            enemy.isBurst = false;
        }
        else
        {
            // 敵の行動処理を実装
            float damage = Random.Range(enemy.AT - 5, enemy.AT + 5);
            float GetElement = BattleData.Instance.GetElementalMultiplier(enemy.element, targetPlayer.element); //属性確認
            Debug.Log(damage);
            Debug.Log(Mathf.Floor(damage * GetElement));

            if(targetPlayer.armor.Count == 0)
            {
                Debug.Log("防具ないよ");
                damage -= targetPlayer.defence;
            }
            else
            {
                Debug.Log("防具あるよ");
                damage -= (targetPlayer.armor[0].number + targetPlayer.defence);
            }
            
            if (damage < 0) damage = 0;

            // 攻撃演出をここでいれたい
            anim = enemy.GetComponent<Animator>();
            StartCoroutine(PlayAttackAnimation());

            ClearBattleLog();

            
            battleLog.text +=  $"\n<color=#{colorCode}>{PlayerObject[playernumber].name}が{damage}のダメージを受けた!</color>";
            Debug.Log($"{targetPlayer.name} に {damage} のダメージ！");
            targetPlayer.TakeDamage(Mathf.Floor(damage * GetElement));

            yield return new WaitForSeconds(2f); // 遅延
        }


    }

    void OnActionSelected(string action)//ボタンを押されるまでターンを止める
    {
        Debug.Log($"選択されたアクション: {action}");
        actionSelected = true; // ボタンが押されたことを通知
    }

    public void OnActionSelected()//ボタンが押されるまでターンを止める
    {
        attackakuction = true; // ボタンが押されたことを通知
        if(attackbotton.activeSelf == true)
        {
            attackbotton.SetActive(!attackbotton.activeSelf);
            escapebotton.SetActive(!escapebotton.activeSelf);
            guagebutton.SetActive(!guagebutton.activeSelf);
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

    void SetupBattle()//キャラクターや敵を生成
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
            cameraMove.SetUp(enemySpawnPoint);
            cameraMove.rotationcamera(2f);
            cameraMove.zoingoutcamera(4f,1f);
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


    private IEnumerator RastLog()//+************************************************
    {
        while(stayturn == true || LebelupStay == true)
        {
            yield return null;
        }

        StartCoroutine(BattleData.Instance.FadeInBuluck(1.5f));

        battleLog.text = $"\nあなたたちの勝ちでーす";
        battleLog.text += $"\nあなたたちの勝ちでーす";
        battleLog.text += $"\nあなたたちの勝ちでーす";
        yield return new WaitForSeconds(1f);

        // フィールドシーンに戻る
        StartCoroutine(BattleData.Instance.LoadMap());
        //SceneManager.LoadScene("GAMEMAPP");
    }

    private void ShowPlayerAndGameOver()
    {
        // "GAME OVER" テキストを表示
        gameOverText.gameObject.SetActive(true);
    }

    void SetupPlayers()//プレイヤーにスペシャル技を付与
    {
        foreach(Player player in players)
        {
            switch (player.job)//************************************リストより重いが,早い。アイテム等の処理は使うとよいだろう
            {
                case Job.None:
                    break;
                case Job.Worrier:
                    player.SetSpecialSkill(player.gameObject.AddComponent<AOESpecial>());
                    break;
                case Job.Magic:
                    player.SetSpecialSkill(player.gameObject.AddComponent<HealAllSpecial>());
                    break;
            }
        }
    }

    public IEnumerator LevelUp(string beforeStr, string afterStr, string StatsName)//レベルアップしたときの特別なログ
    {
        LebelupStay = true;
        LevelupPanel.SetActive(true);
        transparentButton.SetActive(true);

        GameObject Log = Instantiate(LevelupLog, LevelupPanel.transform);
        LvLog.Add(Log);
        Text LVLog = Log.GetComponent<Text>();
        Log.GetComponent<TextAnimation>().SetText($"{StatsName}は{beforeStr} => {afterStr}に上がった！", LVLog);
        //LVLog.text = $"{StatsName}は{beforeStr} => {afterStr}に上がった！";
        yield return null;
    }
    public void ItaDeret()
    {
        foreach (GameObject GO in LvLog)
        {
            if (GO != null)
            {
                Destroy(GO);
            }
            else
            {
                Debug.LogWarning("すでにnullになっています");
            }
        }

        LvLog.Clear(); // リストもクリア
    }
    
    public void StatusOver()//ステータスの更新処理******いずれ他の更新処理に変えたい
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
                    player2.Mp = player.Mp;
                    player2.LV = player.LV;
                    player2.XP = player.XP;
                    player2.MaxXp = player.MaxXp;
                    player2.gold = player.gold;
                    player2.currentGauge = player.currentGauge;
                    player2.job = player.job;
                }
            }
        }
    }

    public void EndBattle()//勝ったとき***************************************
    {
        // BattleDataの情報をリセット（必要に応じて）
        BattleData.Instance.modorusyori();

        foreach(Player player in players)
        {
            player.RemoveBuffe();
        }

        StatusOver();
        BattleData.Instance.SetEnemyData( "", 0);

        //勝ったときのログ
        StartCoroutine(RastLog());

    }

    void EndBattle2(bool isPlayerWin)//死んだとき
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

    private IEnumerator EndBattleSequence()//プレイヤーが死んだときの処理
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
