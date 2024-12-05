using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private EnemyCounter enemyCounter;
    public float health = 50;

    public float maxHealth = 50f;
    public float currentHealth;

    private HealthBarManager healthBarManager;

    public static Enemy selectedEnemy; // 選択された敵を記録する静的変数

    private ArrowManager arrowManager;

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

        if (currentHealth <= 0)
        {
            Die();
        }

        if (health <= 0)
        {
            Debug.Log("やられました");
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
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
