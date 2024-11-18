using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackScript : MonoBehaviour
{
    public Button attackButton;

    public GameObject projectilePrefab; // 弾丸のPrefab
    //スタートの時に発射位置をfindで探すようにしてもいいかも
    public Transform firePoint; // 発射位置
    public float fireRate = 0.5f; // 発射速度
    private float nextFireTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        // ボタンにクリックイベントを登録
        attackButton.onClick.AddListener(OnAttackButtonPressed);
    }

    void OnAttackButtonPressed()
    {
        // 攻撃処理を呼び出す
        Attack();
    }

    void Attack()
    {
        // 攻撃アクションの処理（例：コンソールにメッセージを表示）
        Debug.Log("攻撃！敵にダメージを与えました。");
        
        Shoot();
        nextFireTime = Time.time + fireRate;
        // 実際の攻撃ロジックをここに実装します
        // 例：弾を発射する、敵のHPを減らす等
    }

    void OnDestroy()
    {
        // オブジェクトが破棄されるときにリスナーを解除
        attackButton.onClick.RemoveListener(OnAttackButtonPressed);
    }

    void Shoot()
    {
        // 弾丸を発射位置に生成し、向きを設定
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
