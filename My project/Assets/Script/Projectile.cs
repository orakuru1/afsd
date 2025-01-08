using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 20f; // 弾丸の速度
    public int damage = 10; // 弾丸のダメージ量
    public float lifetime = 2f; // 弾丸が自動で消えるまでの時間
    private BattleManager battleManager;   
    void Start()
    {
        // 一定時間後に弾丸を削除
        Destroy(gameObject, lifetime);        
    }

    // Update is called once per frame
    void Update()
    {
        // 弾丸を前方に進める
        transform.Translate(Vector3.forward * speed * Time.deltaTime);        
    }
    private void OnTriggerEnter(Collider other)
    {
        // 敵に当たった場合の処理
        if (other.CompareTag("Enemy"))
        {
            // Enemyスクリプトを取得し、ダメージを与える
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                //battleManager = FindObjectOfType<BattleManager>();
                //battleManager.PlayerAttack(15);
                //enemy.TakeDamage(damage);
            }
            // 弾丸を削除
            Destroy(gameObject);
        }
    }
}
