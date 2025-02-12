using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyRangedAttack : MonoBehaviour
{
    public Transform player; // プレイヤーのTransform
    public GameObject projectilePrefab; // 発射する弾のPrefab
    public Transform firePoint; // 弾を発射する位置
    public float detectionRange = 20f; // 検知範囲
    public float detectionrange = 20f;
    public float attackCooldown = 1.5f; // 攻撃間隔
    public float projectileSpeed = 10f; // 弾の速度

    private Animator anim; //アニメーション

    public GameObject hpBarCanvas; //HPバーのCanvas
    public Image hpFillImage; //前景のImage(緑部分)

    private float lastAttackTime; // 最後に攻撃した時間


    public void bbbb()
    {
        //プレイヤーをタグで自動取得
        GameObject playerObject = GameObject.FindWithTag("Player");
        if(playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.Log("見つかりません");
        }
    }
    

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if(player!=null)
        {
        // プレイヤーとの距離を計算
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            // プレイヤーをターゲットとして向きを調整
            AimAtPlayer();
            hpBarCanvas.SetActive(true);

            // 一定間隔で攻撃
            if (Time.time > lastAttackTime + attackCooldown)
            {
                AttackPlayer();
            }
        }
        if (distanceToPlayer <= detectionrange)
        {
            // プレイヤーをターゲットとして向きを調整
            AimAtPlayer();
            anim.SetBool("attack", true);
        }
        else{
            hpBarCanvas.SetActive(false);
            anim.SetBool("attack", false);
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

        Debug.Log("プレイヤーに向けて攻撃しました！");
        Destroy(projectile, 3f);
    }
}

