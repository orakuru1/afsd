using UnityEngine;

public class Weapons : MonoBehaviour
{
    public int damage = 10; // 攻撃力

    void OnTriggerEnter(Collider other)
    {
        // 衝突したオブジェクトに「Enemy」スクリプトがあるか確認
        EnemyHP enemy = other.GetComponent<EnemyHP>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage); // ダメージを与える
        }
    }
}

