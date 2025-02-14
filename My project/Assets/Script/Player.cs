using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

[System.Serializable]
public class Skill //攻撃する表示のスキルたち、４個ぐらいかな？
{
    public string skillName; //スキルの名前
    public int damage; //ダメージ量
    public string description; //説明
    public BuffType buffType; // 付与するバフの種類
    public int buffValue; // バフの上昇値
    public int buffDuration; // バフの持続ターン数
}
[System.Serializable]
public class Weapon  //防具や武器のステータス・・・・・・0番が武器として機能している。１番が防具として機能している
{
    public string equipName; //装備の名前
    public int number; //装備の攻撃力
    public string description; //装備の説明
    public int price; //装備の値段
    public Sprite BuckSprite;
}
[System.Serializable]
public class Armor
{
    public string armorname; //装備の名前
    public int number; //装備の防御力
    public string description; //装備の説明
    public int price; //装備の値段
    public Sprite BuckSprite;
}
public enum BuffType//*****************見た目がわかりやすく可読性が増すであろう。数字などで管理しているときは使ってみてもよい。多くなりすぎるとやばい。switch分
{
    None,       // バフなし
    AttackUp,   // 攻撃力アップ
    DefenseUp,  // 防御力アップ
    SpeedUp     // スピードアップ
}
public enum Job
{
    None,
    Worrier,
    Magic,
    Seef
}

public class Player : MonoBehaviour
{
    public List<Skill> skills = new List<Skill>(); //スキルが入ってるリスト
    public List<Weapon> weapon = new List<Weapon>(); //装備が入ってるリスト
    public List<Armor> armor = new List<Armor>(); //装備が入ってるリスト

    [Header("ステータスここから")]
    [SerializeField]public Job job; //役職
    [SerializeField]private string Spn;
    [SerializeField]public string pn;
    [SerializeField]public float health; //死んだ処理のHP
    [SerializeField]public float maxHealth;//一緒になってる
    [SerializeField] public int attack; //攻撃力
    [SerializeField] public int defence; //防御力
    [SerializeField] public int Speed;
    [SerializeField] public int Mp;
    [SerializeField] public int LV; //現在のレベル
    [SerializeField] public double XP; //現在の経験値
    [SerializeField] public double MaxXp; //次のレベルアップの値
    [SerializeField]public float currentHealth; //HPバーに反映される値　(前のHPとごっちゃになった)
    [SerializeField]public int gold; // プレイヤーの初期ゴールド
    [SerializeField]public float currentGauge; // 現在のゲージ値
    [SerializeField]public float maxGauge;
    public float sharp;
    [SerializeField]public Sprite sprite;

    //*************************ここまでプレイヤーのステータス
    public static bool attackmotion = false;
    public static bool damagemotion = false;
    public static bool diemotion = false;
    private bool isGrounded; //プレイヤーが地面にいるかどうか
    public bool isDead{ get; private set;} = false; //死亡フラグ

    public float fallThreshold = -30.0f; //落下とみなすY座標のしきい値
    public float respawnUpdateDistance = 1.0f; //リスポーン位置を更新する間隔（メートル単位）
    public float smoothSpeed = 0.5f; //HPバーが減る速度（小さいほど遅い）
    private float targetSliderValue; //スライダーの目標値

    private Animator anim; //アニメション
    public Slider healthSlider; //HPバー
    public Text healthText; //HPのテキスト表示

    private HealthBarManager healthBarManager; //HPバーを管理するスクリプト

    private BattleManager battleManager; //ターン制バトルを管理するスクリプト
    
    private SpecialSkill specialSkill;

    private CameraMove cameraMove;
    public Vector3 offset;         // カメラのオフセット（キャラクターからの位置）
    public float rotationSpeed = 5.0f;


    private Vector3 respawnPosition; //リスポーン位置を記録する変数

    //public event System.Action OnStatsUpdated; //オブサーバ、デザインパターン
    private Color color;
    private Dictionary<BuffType, int> ActiveBuffs = new Dictionary<BuffType, int>();
    private Dictionary<BuffType, int> ActiveBuffs2 = new Dictionary<BuffType, int>();//dictionaryとenumのコンビは相性がいいと思います
    public void ApplyBuff(BuffType buffType, int value, int duration)
    {
        if (buffType == BuffType.None) return; // バフなしなら処理しない

        if (ActiveBuffs.ContainsKey(buffType))
        {
            ActiveBuffs[buffType] = Mathf.Max(ActiveBuffs[buffType], duration); // 持続ターンを更新
            ActiveBuffs2[buffType] = Mathf.Max(ActiveBuffs2[buffType], value); // 持続ターンを更新
        }
        else
        {
            ActiveBuffs.Add(buffType, duration);
            ActiveBuffs2.Add(buffType,value);
            switch (buffType)//************************************リストより重いが早い。アイテム等の処理は使うとよいだろう
            {
                case BuffType.AttackUp:
                    attack += ActiveBuffs2[buffType];
                    break;
                case BuffType.DefenseUp:
                    defence += ActiveBuffs2[buffType];
                    break;
                case BuffType.SpeedUp:
                    Speed += ActiveBuffs2[buffType];
                    break;
            }
        }


        Debug.Log($"{gameObject.name} に {buffType} のバフ（+{value}）を適用！ {duration}ターン持続");
    }

    public void UpdateBuffs()
    {
        List<BuffType> buffsToRemove = new List<BuffType>();

        foreach (var buff in ActiveBuffs.Keys.ToList()) 
        {
            ActiveBuffs[buff]--;

            if (ActiveBuffs[buff] <= 0)
            {
                buffsToRemove.Add(buff);
            }
        }

        foreach (var buff in buffsToRemove)//*********************バトルが終わった時に、まだバフがついていたら解除してから戻る
        {
            switch (buff)
            {
                case BuffType.AttackUp:
                    attack -= ActiveBuffs2[buff];
                    break;
                case BuffType.DefenseUp:
                    defence -= ActiveBuffs2[buff];
                    break;
                case BuffType.SpeedUp:
                    Speed -= ActiveBuffs2[buff];
                    break;
            }

            ActiveBuffs.Remove(buff);
            ActiveBuffs2.Remove(buff);
            Debug.Log($"{gameObject.name} の {buff} バフが消えた！");
        }
    }
    public int GetBuffedStat(BuffType type)  //現在のバフが何ターンか;
    {
        return ActiveBuffs.ContainsKey(type) ? ActiveBuffs[type] : 0;
    }
    public void RemoveBuffe()
    {
        foreach(var buff in ActiveBuffs.Keys)
        {
            switch(buff)
            {
                case BuffType.AttackUp:
                    attack -= ActiveBuffs2[buff];
                    break;
                case BuffType.DefenseUp:
                    defence -= ActiveBuffs2[buff];
                    break;
                case BuffType.SpeedUp:
                    Speed -= ActiveBuffs2[buff];
                    break;
            }
        }
    }
    public void SetSpecialSkill(SpecialSkill skill) //継承元の親だけでいい
    {
        specialSkill = skill;
        skill.Initialize(this,battleManager);
    }
    public void SetUpBattleManager(BattleManager mana) //battlemanagerをゲット
    {
        battleManager = mana;
    }
    public void SetUpSPN(GameObject gameObject)
    {
        gameObject.GetComponentInChildren<Text>().text = Spn;
    }

    public void GetGolrd(int StealGorld) //プレイヤースクリプト(親元)のお金を増やす処理
    {
        gold += StealGorld; //お金の計算
    }

    public bool SpendGold(int amount) //ショップで買う時の処理
    {
        if (gold >= amount) //お金が値段より持ってるかどうか
        {
            gold -= amount; //所持金を減らす(親元)　　　　(いつか上の関数に変える)
            Debug.Log($"ゴールドを消費: {amount}, 残り: {gold}"); 
            return true; //持っていたと報告
        }
        else
        {
            Debug.Log("ゴールドが足りません！");
            return false; //持っていなかったと報告
        }
    }

    public void LevelUp(int experience) //経験値習得     
    {
        Debug.Log("経験値きたー");
        float prevHealth = health;
        float prevMaxHealth = maxHealth;
        int prevAttack = attack;
        int prevDefence = defence;
        int prevSpeed = Speed;
        int prevMp = Mp;
        int prevLV = LV;

        XP += experience; //今の経験値に送られてきた経験値の計算
        battleManager.AddLog($"{pn}は{experience}の経験値を得た!");
        
        if(XP >= MaxXp) //レベルアップに達しているかどうか
        {
            health += 50; //HPアップ
            currentHealth += 50; //バーのHPもアップ
            maxHealth += 50; //最大HPもアップ

            if(healthText != null)
            {
                healthText.text = "healthText" + 50; //テキストの数もアップ
            }

            attack += 20; //攻撃力アップ
            defence += 10; //防御力アップ
            double AfterXp = XP - MaxXp;
            XP = 0; //現在の経験値を０に更新　　　　(オーバーした経験値を引き継げるようにするかは考える)
            MaxXp *= 1.2; //次のレベルアップまでを更新する
            LV += 1; //現在のレベルを上げる

            switch(job)
            {
                case(Job.None):
                    break;
                case(Job.Worrier):
                    health += 20;
                    currentHealth += 20;
                    maxHealth += 20;
                    attack += 20;
                    break;
                case(Job.Magic):
                    Mp += 20;
                    Speed += 20;
                    break;
                case(Job.Seef):
                    Speed += 20;
                    attack += 10;
                    break;
            }

            
            if (LV > prevLV) StartCoroutine(battleManager.LevelUp(prevLV.ToString(), LV.ToString(), $"{pn}のレベル"));
            if (health > prevHealth) StartCoroutine(battleManager.LevelUp(prevHealth.ToString(), health.ToString(), "体力"));
            if (maxHealth > prevMaxHealth) StartCoroutine(battleManager.LevelUp(prevMaxHealth.ToString(), maxHealth.ToString(), "最大値"));
            if (attack > prevAttack) StartCoroutine(battleManager.LevelUp(prevAttack.ToString(), attack.ToString(), "攻撃力"));
            if (defence > prevDefence) StartCoroutine(battleManager.LevelUp(prevDefence.ToString(), defence.ToString(), "防御力"));
            if (Speed > prevSpeed) StartCoroutine(battleManager.LevelUp(prevSpeed.ToString(), Speed.ToString(), "速度"));
            if (Mp > prevMp) StartCoroutine(battleManager.LevelUp(prevMp.ToString(), Mp.ToString(), "魔力"));

            if(AfterXp >= 0)
            {
                Debug.Log("繰り越し");
                LevelUp((int)AfterXp);
            }

        }

        //OnStatsUpdated?.Invoke(); // 通知を送信
        battleManager.StatusOver();
        UpdateHealthBar();

        //BattleData.Instance.SetPlayerStatus(pn,health,maxHealth,attack,defence,Speed,LV,XP,MaxXp,currentHealth);
    }

    public void escape() //逃げるボタンを押されたときの処理
    {
        double ran = Random.Range(1,10); //１～９までのランダム数字(多分)
        if(ran < 4)
        {
            BattleData.Instance.modorusyori();
            SceneManager.LoadScene("SampleScene"); //元のシーンに戻る
        }
        else
        {
            Debug.Log("逃げられなかった!");
        }
    }
    public void Attack(Skill skill,Player player,GameObject panel) //プレイヤーの攻撃処理
    {
        attackmotion = true;
        // 選択された敵を攻撃
        Enemy targetEnemy = Enemy.selectedEnemy; //現在クリックされてる敵を習得

        if (targetEnemy == null) //敵がクリックされてるかどうか
        {
            // 敵が選択されていない場合は、一番左の敵を攻撃
            targetEnemy = EnemyManager.GetLeftMostEnemy();
        }

        if (targetEnemy != null) //敵がクリックされてる
        {
            Debug.Log($"攻撃！ {targetEnemy.gameObject.name} を攻撃します。");
            StartCoroutine(ExecuteAttack(targetEnemy,skill,player)); //選択されてる敵の情報と、生成されたスキルボタンの内容を送る
        }
        else
        {
            Debug.Log("攻撃する敵がいません！");
        }

        // 攻撃後、選択状態をリセット
        //Enemy.selectedEnemy = null;
        Invoke(nameof(StopAttack), 0.1f);
        Destroy(panel);
    }
    private IEnumerator ExecuteAttack(Enemy target,Skill skill,Player player) //実際に攻撃するところ
    {
        Debug.Log($"{this.name} は {skill.skillName} を使用！");

        if (skill.buffType != BuffType.None) // バフがある場合
        {
            battleManager.ClearBattleLog();
            player.ApplyBuff(skill.buffType, skill.buffValue, skill.buffDuration);
            battleManager.AddLog(skill.buffType+"で"+skill.buffValue+"の効果がアップした!");
        }
        else
        {
            cameraMove.CharacterToEnemy(this.transform.position);
            cameraMove.SetUp(target.gameObject.transform); //カメラが敵を向くように
            battleManager.ClearBattleLog();
            battleManager.AddLog($"{target.gameObject.name}を攻撃!!");

            yield return new WaitForSeconds(1f); //アニメーションとか入れれるかも。

            int damage = 0;
            if(player.weapon.Count == 0)
            {
                Debug.Log("武器はないよ");
                damage = Random.Range(player.attack + skill.damage, player.attack + skill.damage);
            }
            else
            {
                Debug.Log("武器はあるよ");
                damage = Random.Range(player.attack + skill.damage + weapon[0].number, player.attack + skill.damage + weapon[0].number); //自分の攻撃力とスキルのダメージと武器のダメージをランダムで幅を出そうとしてる
            }

            target.GetComponent<Enemy>()?.TakeDamage(damage,player); //敵に攻撃を送ってる
            EnemyDestroyGuage eneguage = target.GetComponent<EnemyDestroyGuage>();
            eneguage.FillGauge(sharp);
            
            yield return new WaitForSeconds(1f);//ここに敵にダメージを与えるアニメーションが終わるまでやらないループを作りたい

            StartCoroutine(cameraMove.ComeBuckCamera());

            battleManager.ClearBattleLog();
            foreach(var type in ActiveBuffs.Keys)  //バフがたくさんあったらバトルログに入りきらない
            {
                if(GetBuffedStat(type) != 1) //switch文でかいてもいい
                {
                    battleManager.AddLog("残りのバフ継続ターン数"+GetBuffedStat(type).ToString()+"ターンです");
                }
                else
                {
                    battleManager.AddLog(GetBuffedStat(type).ToString()+"のバフが切れた!");
                }
            }
        }

        yield return null;

    }
    public void OnSpecialAction(Player player)                    ////////////////スペシャル技///////////////
    {
        if(player.currentGauge >= player.maxGauge)
        {
            if(specialSkill != null)
            {
                specialSkill.Activate();
            }
            else
            {
                Debug.Log("設定されていません");
            }
        }
        else
        {
            Debug.Log("エネルギーが足りません");
        }
    }

    public void TakeDamage(int damage) //自分のダメージを受ける処理
    {
        Debug.Log(this.gameObject);
        StartCoroutine("MDamage");
        health -= damage; //HPにダメージを食らう
        if (health < 0) health = 0; //０より下になった時に０にする

        currentHealth -= damage; //ＨＰばーも減らす
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // HPを範囲内に制限

        // スライダーの目標値を計算
        targetSliderValue = (float)currentHealth / maxHealth;

        if (currentHealth < 0) currentHealth = 0; //０より下になった時に０にする
        
        if(healthBarManager != null)
        {
            UpdateHealthBar(); //HPバーが減ったから更新する
        }

        if(healthText != null)
        {
            healthText.text = currentHealth.ToString(); //HPテキスト処理
        }
        
        if (currentHealth <= 0) //ＨＰがなくなったら
        {
            diemotion = true;
            Die(); //死んだときの処理
        }

        Debug.Log("プレイヤーの体力: " + health);
        Invoke(nameof(StopDamage), 0.2f);
    }

    public void Heal(float amount) //HPを回復させる処理
    {
        currentHealth += amount; //ＨＰを増やす
        health += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth; //最大値を超えたとき最大値に合わせる
        if (health > maxHealth) health = maxHealth; //最大値を超えたとき最大値に合わせる
        UpdateHealthBar(); //ＨＰバーを更新
    }

    private void UpdateHealthBar() //実際のＨＰに反映させる所(中間管理)
    {
        if (healthBarManager != null) //スクリプトがちゃんと存在するか
        {
            healthBarManager.UpdateHealth(currentHealth, maxHealth); //違うスクリプトでHPバーを更新してる
        }
    }

    

    void UpdateHealthUI(bool instant = false)
    {
        if (healthSlider != null)
        {
            if (instant)
            {
                healthSlider.value = (float)currentHealth / maxHealth; // 即座に更新
            }
        }

        if (healthText != null)
        {
            healthText.text = currentHealth.ToString(); // HPテキストを更新
        }
    }

    private void Die() //HPが０になった時の処理
    {
        Debug.Log($"{gameObject.name} が倒されました！");
        isDead = true;
        anim.SetBool("die", true);
        //Destroy(gameObject);
    }

    void OnDestroy() //破壊されたときの処理   ※シーンが遷移された時にもされてしまう
    {
        BattleManager.players.Remove(this);
    }

    void StopAttack()
    {
        attackmotion = false;
    }

    void StopDamage()
    {
        damagemotion = false;
    }

    IEnumerator MDamage()
    {
        damagemotion = true;
        yield return new WaitForSeconds(4f);
    }

    IEnumerator SmoothHealthBarChange()
    {
        while (Mathf.Abs(healthSlider.value - targetSliderValue) > 0.01f)
        {
            healthSlider.value = Mathf.Lerp(healthSlider.value, targetSliderValue, smoothSpeed);
            yield return null; // フレームを待つ
        }

        // 最後に値を正確に設定（誤差補正）
        healthSlider.value = targetSliderValue;
    }

    //リスポーン位置に戻す
    void Respawn()
    {
        transform.position = respawnPosition;
        Debug.Log("落下したのでリスポーンしました！");
    }

    //地面に触れた時の処理
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    //地面から離れた時の処理
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    //プレイヤーが安全な位置を通過したらリスポーン地点を更新
    /*private void OnTriggerEnter(Collider other)
    {
        //地面やチェックポイントに触れたらリスポーン位置を更新
        if(other.CompareTag("Ground") || other.CompareTag("Checkpoint"))
        {
            respawnPosition = transform.position;
            Debug.Log("新しいリスポーン地点を設定しました：" + respawnPosition);
        }
    }*/

    void UpdateRespawnposition()
    {
        respawnPosition = transform.position;
        //Debug.Log("リスポーン位置を更新："　+ respawnPosition);
    }

    void LateUpdate()
    {
    if (Input.GetMouseButton(1)) // 右クリックで回転を有効にする場合
    {
        float horizontal = Input.GetAxis("Mouse X") * rotationSpeed;
        offset = Quaternion.AngleAxis(horizontal, Vector3.up) * offset;
    }
    }

    void Start()
    { 
        cameraMove = Camera.main.GetComponent<CameraMove>();
        //currentHealth = maxHealth;
        healthBarManager = GetComponent<HealthBarManager>(); //自分に追加されてるはずのＨＰバーのスクリプトを使えるようにしてる
        UpdateHealthBar(); //現在のＨＰを反映(最初からＨＰが減ってるときのため)
        anim = GetComponent<Animator>();
        //skills.Add(new Skill { skillName = "Fireball", damage = 30, description = "A ball of fire that burns enemies." });
        respawnPosition = transform.position; //初期位置をリスポーン位置として記録
    }              

    // Update is called once per frame
    void Update()
    {
       

        //一定距離移動したらリスポーン位置を更新
        if(isGrounded && Vector3.Distance(respawnPosition, transform.position) >= respawnUpdateDistance)
        {
            UpdateRespawnposition();
        }
        //現在の座標を確認
        if(transform.position.y < fallThreshold)
        {
            Respawn();
        }
        attackmotion = false;
    }
}
