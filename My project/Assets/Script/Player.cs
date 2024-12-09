using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    [SerializeField]public int health;
    [SerializeField]public float maxHealth;//一緒になってる

    [SerializeField] public int attack;
    [SerializeField] public int defence;
    [SerializeField] public int LV;
    [SerializeField] public double XP;
    [SerializeField] public double MaxXp;
    [SerializeField]public float currentHealth;

    private HealthBarManager healthBarManager;

    private BattleManager battleManager;
    [SerializeField]private Player player;

    public event System.Action OnStatsUpdated;

    public void LevelUp(int experience) //経験値習得
    {
        XP += experience;
        if(XP >= MaxXp)
        {
            health += 50;
            currentHealth += 50;
            maxHealth += 50;
            attack += 20;
            defence += 10;
            XP = 0;
            MaxXp *= 1.2;
            LV += 1;
        }
        OnStatsUpdated?.Invoke(); // 通知を送信
    }
    public void Attack()
    {
        // 選択された敵を攻撃
        Enemy targetEnemy = Enemy.selectedEnemy;

        if (targetEnemy == null)
        {
            // 敵が選択されていない場合は、一番左の敵を攻撃
            targetEnemy = EnemyManager.GetLeftMostEnemy();
        }

        if (targetEnemy != null)
        {
            Debug.Log($"攻撃！ {targetEnemy.gameObject.name} を攻撃します。");
            ExecuteAttack(targetEnemy);
        }
        else
        {
            Debug.Log("攻撃する敵がいません！");
        }

        // 攻撃後、選択状態をリセット
        //Enemy.selectedEnemy = null;
    }

    private void ExecuteAttack(Enemy target)
    {
        int damage = Random.Range(BattleManager.players[0].attack,BattleManager.players[0].attack);
        // 攻撃処理（例: 敵にダメージを与える）
        target.GetComponent<Enemy>()?.TakeDamage(damage); //１０の部分は攻撃力の参照にできそう
        battleManager = FindObjectOfType<BattleManager>();
        battleManager.PlayerAttack(damage);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health < 0) health = 0;
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }

        Debug.Log("プレイヤーの体力: " + health);
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
            Debug.Log(currentHealth);
            Debug.Log(maxHealth);
            healthBarManager.UpdateHealth(currentHealth, maxHealth);
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} が倒されました！");
        //Destroy(gameObject);
    }

    void OnDestroy()
    {
        BattleManager.players.Remove(this);
    }

    void Start()
    {
        //currentHealth = maxHealth;
        healthBarManager = GetComponent<HealthBarManager>();
        UpdateHealthBar();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
