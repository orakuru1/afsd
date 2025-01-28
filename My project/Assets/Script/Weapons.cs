using UnityEngine;

public class Weapons : MonoBehaviour
{
    public int damage = 10; // 攻撃力
    private bool hasHit = false; //一度攻撃したかのフラグ

    void OnTriggerEnter(Collider other)
    {
        if(hasHit) return; //攻撃が一回のみ有効

        // 衝突したオブジェクトに「Enemy」スクリプトがあるか確認
        EnemyHP enemy = other.GetComponent<EnemyHP>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage); // ダメージを与える
            hasHit = true; //攻撃済みフラグを設定
        }
    }

    void OnTriggerExit(Collider other)
    {
        //コライダーを離れたらフラグをリセット
        hasHit = false;
    }
}

