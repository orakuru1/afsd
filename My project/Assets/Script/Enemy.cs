using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private EnemyCounter enemyCounter;
    public float health = 50; //HP
    public float maxHealth = 50f; //UIのHP

    [SerializeField]public int AT;
    [SerializeField]public int DF;
    [SerializeField]public int Speed;
    
    [SerializeField]private int EXP = 50; //経験値
    public float currentHealth;

    private HealthBarManager healthBarManager;

    public static Enemy selectedEnemy; // 選択された敵を記録する静的変数

    private ArrowManager arrowManager;
    private BattleManager battleManager;
    [SerializeField] Player player;
    [SerializeField] int DropGorld;

    void OnMouseDown()
    {
        // 敵がクリックされたときに、この敵を選択状態にする
        selectedEnemy = this;
        Debug.Log($"{gameObject.name} が選択されました。");
    }

    public void TakeDamage(int damage)
    {
        damage -= DF;
        if (damage < 0) damage = 0; //敵が攻撃を受けたときに敵の防御力を反映している
        battleManager = FindObjectOfType<BattleManager>();
        battleManager.hyouzi(damage);
        
        health -= damage;
        Debug.Log(health);
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
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

    private void Die()
    {
        // 敵が死亡する処理（例: エフェクトやスコアの増加など）
        BattleManager.players[0].LevelUp(EXP);
        player.GetGolrd(DropGorld);
        Destroy(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
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
        if (EnemyManager.enemies.Contains(enemy)) //findで探して敵を探してる
        {
            EnemyManager.enemies.Remove(enemy); //エネミーの情報を消す
        }
        if(BattleManager.enemys.Contains(enemy)) //インスタンスしたときの情報を持ってる
        {
            BattleManager.enemys.Remove(enemy); //エネミーの情報を消す
        }

    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
