using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int attackDamage = 10; //敵の攻撃力
    public float attackCooldown = 1.5f; //攻撃間隔
    private float lastAttackTime; //最後に攻撃した時間
 
    private PlayerHealth playerHealth; // プレイヤーのHPスクリプト参照
    // Start is called before the first frame update

    void OnCollisionEnter(Collision collision)
    {
        //衝突したオブジェクトがプレイヤーの場合
        if(collision.gameObject.CompareTag("Player") && playerHealth != null && !playerHealth.isDead)
        {
            TryAttackPlayer(); // 攻撃処理を呼び出す
        }
    }
    void Start()
    {
        // プレイヤーオブジェクトを取得し、HP管理スクリプトを参照
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
        }
    }

    void OnCollisionStay(Collision collision)
    {
        // プレイヤーが触れている間、定期的に攻撃する
        if (collision.gameObject.CompareTag("Player") && playerHealth != null && !playerHealth.isDead)
        {
            TryAttackPlayer(); // 攻撃処理を呼び出す
        }
    }

    void TryAttackPlayer()
    {
        // 一定間隔ごとに攻撃
        if (Time.time > lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time; // 攻撃時間をリセット
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage); // プレイヤーにダメージを与える
                Debug.Log("Player attacked!");
            }
        }
    }
}
