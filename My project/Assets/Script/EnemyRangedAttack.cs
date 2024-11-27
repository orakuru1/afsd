using UnityEngine;

public class EnemyRangedAttack : MonoBehaviour
{
    public Transform player; // プレイヤーのTransform
    public GameObject projectilePrefab; // 発射する弾のPrefab
    public Transform firePoint; // 弾を発射する位置
    public float detectionRange = 20f; // 検知範囲
    public float attackCooldown = 1.5f; // 攻撃間隔
    public float projectileSpeed = 10f; // 弾の速度

    private float lastAttackTime; // 最後に攻撃した時間

    void Update()
    {
        // プレイヤーとの距離を計算
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            // プレイヤーをターゲットとして向きを調整
            AimAtPlayer();

            // 一定間隔で攻撃
            if (Time.time > lastAttackTime + attackCooldown)
            {
                AttackPlayer();
            }
        }
    }

    void AimAtPlayer()
    {
        // プレイヤーの位置をターゲットにして敵の向きを設定
        Vector3 targetPosition = player.position;
        transform.LookAt(targetPosition);
    }

    void AttackPlayer()
    {
        // 弾を生成
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // 弾に力を加えてプレイヤーに向かわせる
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        Vector3 direction = (player.position - firePoint.position).normalized;
        rb.velocity = direction * projectileSpeed;

        lastAttackTime = Time.time; // 攻撃間隔のリセット

        Debug.Log("空中のプレイヤーに向けて攻撃しました！");
    }
}

