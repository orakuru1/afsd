using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Skill //攻撃する表示のスキルたち、４個ぐらいかな？
{
    public string skillName; //スキルの名前
    public int damage; //ダメージ量
    public string description; //説明
}
[System.Serializable]
public class Weapon  //防具や武器のステータス・・・・・・0番が武器として機能している。１番が防具として機能している
{
    public string equipName; //装備の名前
    public int number; //装備の攻撃力
    public string description; //装備の説明
    public int price; //装備の値段
}
[System.Serializable]
public class Armor
{
    public string armorname; //装備の名前
    public int number; //装備の防御力
    public string description; //装備の説明
    public int price; //装備の値段
}

public class Player : MonoBehaviour
{
    public List<Skill> skills = new List<Skill>(); //スキルが入ってるリスト
    public List<Weapon> weapon = new List<Weapon>(); //装備が入ってるリスト
    public List<Armor> armor = new List<Armor>(); //装備が入ってるリスト
    [SerializeField]public string pn;
    [SerializeField]public int health; //死んだ処理のHP
    [SerializeField]public float maxHealth;//一緒になってる
    [SerializeField] public int attack; //攻撃力
    [SerializeField] public int defence; //防御力
    [SerializeField] public int Speed;
    [SerializeField] public int LV; //現在のレベル
    [SerializeField] public double XP; //現在の経験値
    [SerializeField] public double MaxXp; //次のレベルアップの値
    [SerializeField]public float currentHealth; //HPバーに反映される値　(前のHPとごっちゃになった)
    private HealthBarManager healthBarManager; //HPバーを管理するスクリプト
    public static bool attackmotion = false;
    public static bool damagemotion = false;
    public static bool diemotion = false;
    private BattleManager battleManager; //ターン制バトルを管理するスクリプト

    //public event System.Action OnStatsUpdated; //オブサーバ、デザインパターン
    private BattleSystem battleSystem; //技のボタンを表示・非表示してるとこ(今見てみたら使ってるのかわからんかった)
    [SerializeField]public int gold; // プレイヤーの初期ゴールド

    public void SetUpBattleManager(BattleManager mana)
    {
        battleManager = mana;
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
        XP += experience; //今の経験値に送られてきた経験値の計算
        if(XP >= MaxXp) //レベルアップに達しているかどうか
        {
            health += 50; //HPアップ
            currentHealth += 50; //バーのHPもアップ
            maxHealth += 50; //最大HPもアップ
            attack += 20; //攻撃力アップ
            defence += 10; //防御力アップ
            XP = 0; //現在の経験値を０に更新　　　　(オーバーした経験値を引き継げるようにするかは考える)
            MaxXp *= 1.2; //次のレベルアップまでを更新する
            LV += 1; //現在のレベルを上げる
        }
        //OnStatsUpdated?.Invoke(); // 通知を送信
        battleManager.StatusOver();
        UpdateHealthBar();

        BattleData.Instance.SetPlayerStatus(pn,health,maxHealth,attack,defence,Speed,LV,XP,MaxXp,currentHealth);
    }

    public void escape() //逃げるボタンを押されたときの処理
    {
        double ran = Random.Range(1,10); //１～９までのランダム数字(多分)
        if(ran < 4)
        {
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
            ExecuteAttack(targetEnemy,skill,player); //選択されてる敵の情報と、生成されたスキルボタンの内容を送る
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

    private void ExecuteAttack(Enemy target,Skill skill,Player player) //実際に攻撃するところ
    {
        //int damage = Random.Range(BattleManager.players[0].attack,BattleManager.players[0].attack);
        int damage = Random.Range(player.attack + skill.damage + weapon[0].number,player.attack + skill.damage + weapon[0].number); //自分の攻撃力とスキルのダメージと武器のダメージをランダムで幅を出そうとしてる
        // 攻撃処理（例: 敵にダメージを与える）
        //battleManager = FindObjectOfType<BattleManager>();
        //battleManager.PlayerAttack(damage);
        target.GetComponent<Enemy>()?.TakeDamage(damage,player); //敵に攻撃を送ってる
    }

    public void TakeDamage(int damage) //自分のダメージを受ける処理
    {
        StartCoroutine("MDamage");
        health -= damage; //HPにダメージを食らう
        if (health < 0) health = 0; //０より下になった時に０にする

        currentHealth -= damage; //ＨＰばーも減らす
        if (currentHealth < 0) currentHealth = 0; //０より下になった時に０にする
        
        if(healthBarManager != null)
        {
            UpdateHealthBar(); //HPバーが減ったから更新する
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
        if (currentHealth > maxHealth) currentHealth = maxHealth; //最大値を超えたとき最大値に合わせる
        UpdateHealthBar(); //ＨＰバーを更新
    }

    private void UpdateHealthBar() //実際のＨＰに反映させる所(中間管理)
    {
        if (healthBarManager != null) //スクリプトがちゃんと存在するか
        {
            healthBarManager.UpdateHealth(currentHealth, maxHealth); //違うスクリプトでHPバーを更新してる
        }
    }

    private void Die() //HPが０になった時の処理
    {
        Debug.Log($"{gameObject.name} が倒されました！");
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

    void Start()
    {
        //currentHealth = maxHealth;
        healthBarManager = GetComponent<HealthBarManager>(); //自分に追加されてるはずのＨＰバーのスクリプトを使えるようにしてる
        UpdateHealthBar(); //現在のＨＰを反映(最初からＨＰが減ってるときのため)

        // デバッグ用: リストにスキルを手動で追加
        //skills.Add(new Skill { skillName = "Fireball", damage = 30, description = "A ball of fire that burns enemies." });
        
    }              

    // Update is called once per frame
    void Update()
    {
        attackmotion = false;
    }
}
