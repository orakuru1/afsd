using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private EnemyCounter enemyCounter;
    public int health = 50;
    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log(health);
        if (health <= 0)
        {
            Debug.Log("やられました");
            Die();
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
        // EnemyCounterオブジェクトを探して参照
        enemyCounter = FindObjectOfType<EnemyCounter>();
        if (enemyCounter != null)
        {
            enemyCounter.OnEnemySpawn();
        }
    }
    
    void OnDestroy()
    {
        if (enemyCounter != null)
        {
            enemyCounter.OnEnemyDestroyed();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
