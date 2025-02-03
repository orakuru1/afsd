using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f; // 弾の速度
    public int damage = 10; // 弾のダメージ量
    public float lifetime = 1f; // 弾が自動で消えるまでの時間

    void Start()
    {
        Destroy(gameObject, lifetime); // 一定時間後に弾を削除
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime); // 弾を前方に進める
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("弾が " + other.gameObject.name + " に当たった！");

        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                Debug.Log("プレイヤーにヒット！ダメージを与える: " + damage);
                playerHealth.TakeDamage(damage); // プレイヤーのHPを減らす
            }
            else
            {
                Debug.LogError("PlayerHealth スクリプトが見つかりません！");
            }

            Destroy(gameObject); // 弾を削除
        }
    }
}
