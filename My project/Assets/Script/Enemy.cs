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
    
    [SerializeField]private int EXP = 50; //経験値
    public float currentHealth;

    private HealthBarManager healthBarManager;

    public static Enemy selectedEnemy; // 選択された敵を記録する静的変数

    private ArrowManager arrowManager;
    [SerializeField] Player player;
    Animator anim;
    void OnMouseDown()
    {
        // 敵がクリックされたときに、この敵を選択状態にする
        selectedEnemy = this;
        Debug.Log($"{gameObject.name} が選択されました。");
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log(health);
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        UpdateHealthBar();
        StartCoroutine("PlayDamageAnimation");
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
        if (EnemyManager.enemies.Contains(enemy))
        {
            EnemyManager.enemies.Remove(enemy);
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
