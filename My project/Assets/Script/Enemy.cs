using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private EnemyCounter enemyCounter;
    public Element element;
    public float health = 50; //HP
    public float maxHealth = 50f; //UIのHP
    [SerializeField]private string enemyname;
    public string en;
    [SerializeField]public string Race;
    [SerializeField]public int AT;
    [SerializeField]public int DF;
    [SerializeField]public int Speed;
    [SerializeField] int DropGorld;
    [SerializeField]private int EXP = 50; //経験値
    public ParticleSystem AEfect;
    public float currentHealth;

    private HealthBarManager healthBarManager;

    public static Enemy selectedEnemy; // 選択された敵を記録する静的変数

    private BattleManager battleManager;

    private ArrowManager arrowManager;

    [SerializeField]private Player player;

    public bool isBurst = false;

    Animator anim;

    //public Dictionary<Element, float> CDamage = new Dictionary<Element, float>();
    //public Dictionary<Element, float> CDuration = new Dictionary<Element, float>();

    public List<float> CDamage = new List<float>();

    void OnMouseDown()
    {
        // 敵がクリックされたときに、この敵を選択状態にする
        selectedEnemy = this;
        Debug.Log($"{gameObject.name} が選択されました。");
    }

    public void AddCProbalitiy(float current)
    {
        CDamage.Add(current);
    }

    public void TakeDamage(float damage,Player player, Skill skill)
    {
        damage -= DF;

        if(damage < 0) damage = 0;
        battleManager.hyouzi(damage);

        health -= damage;
        Debug.Log(health);
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        UpdateHealthBar();

        StartCoroutine("PlayDamageAnimation");
        if (currentHealth <= 0)
        {
            Die(player);
        }
    }

    public void SupecialDamage(int damage, Player player)//スペシャル技用ダメージ+++++独自の効果を作る
    {
        battleManager.hyouzi(damage);

        health -= damage;
        Debug.Log(health);
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        UpdateHealthBar();

        StartCoroutine("PlayDamageAnimation");

        if (currentHealth <= 0)
        {
            Die(player);
        }
    }

    public void ContinueDamage(float damage)
    {
        battleManager.AddLog($"{en}は{damage}の継続ダメージを受けた!");

        health -= damage;
        Debug.Log(health);
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        Debug.Log($"{damage}の継続ダメージ！");

        UpdateHealthBar();

        StartCoroutine("PlayDamageAnimation");

        if (currentHealth <= 0)
        {
            Die(BattleManager.LastAttackPlayer);//誰が継続ダメージを送ってきたのかの判別ができない
        }
    }

    public IEnumerator ContinueCheck()//継続ダメージを食らって１ターンごとに減って。無くならせることができるようになった。
    // 同じ属性だったら上書きするか？ステータスのデバフは？継続ダメージを食らっていたらUIに表示するのもあり。プレイヤーには実装していない。
    {
        if(CDamage.Count == 0) yield break;

        List<int> RemoveBox = new List<int>();

        for(int i = CDamage.Count -3; i >= 0; i -= 3)
        {
            ContinueDamage(CDamage[i + 1]);
            CDamage[i + 2] --;

            if(CDamage[i + 2] <= 0)
            {
                RemoveBox.Add(i);
            }
        }

        yield return new WaitForSeconds(1f);

        foreach(int con in RemoveBox)
        {
            CDamage.RemoveAt(con + 2);
            CDamage.RemoveAt(con + 1);
            CDamage.RemoveAt(con);
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (healthBarManager != null)
        {
            healthBarManager.UpdateHealth(currentHealth, maxHealth);
        }
    }

    private void Die(Player player)
    {
        // 敵が死亡する処理（例: エフェクトやスコアの増加など）
        player.GetGolrd(DropGorld);
        player.LevelUp(EXP);
        Destroy(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(triggeroff());
        battleManager = FindObjectOfType<BattleManager>();
        arrowManager = FindObjectOfType<ArrowManager>();
        // EnemyCounterオブジェクトを探して参照
        enemyCounter = FindObjectOfType<EnemyCounter>();
        if (enemyCounter != null)
        {
            enemyCounter.OnEnemySpawn();
        }

        currentHealth = maxHealth;
        healthBarManager = GetComponent<HealthBarManager>();
        UpdateHealthBar();
    }

    private IEnumerator triggeroff()
    {
        yield return new WaitForSeconds(2.0f);
        GetComponent<EnemyTrigger>().TriggerStart();
    }
    
    void OnDestroy()
    {
        if (enemyCounter != null)
        {
            enemyCounter.OnEnemyDestroyed();
            
        }

        // 敵が破壊されたときにArrowManagerのターゲットを更新
        if (arrowManager != null)
        {
            arrowManager.UpdateTarget();
        }

        RemoveEnemy(this);

    }

    public void RemoveEnemy(Enemy enemy)
    {
        if (EnemyManager.enemies.Contains(enemy))
        {
            EnemyManager.enemies.Remove(enemy);
        }
        if(BattleManager.enemys.Contains(enemy))
        {
            BattleManager.enemys.Remove(enemy);
        }
    }

    public IEnumerator PlayDamageAnimation()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("gethit", true);
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("gethit", false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
