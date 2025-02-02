using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f; // 弾の速度
    public int damage = 10; // 弾のダメージ量
    public float lifetime = 1f; // 弾が自動で消えるまでの時間

    void Start()
    {
        // 一定時間後に弾を削除
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // 弾を前方に進める
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // **敵に当たった場合**
        /*if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // ダメージを与える
            }
            Destroy(gameObject); // 弾を削除
        }*/

        // **プレイヤーに当たった場合**
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damage); // プレイヤーのHPを減らす
            }
            Destroy(gameObject); // 弾を削除
        }
    }
}
