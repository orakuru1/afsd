using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    public int maxHP = 100; // 敵の最大HP
    private int currentHP;
   
    void Start()
    {
        currentHP = maxHP; // 初期化
       
    }

    // ダメージを受けるメソッド
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        Debug.Log($"敵の残りHP: {currentHP}");

        if (currentHP <= 0)
        {
            Die(); // HPが0以下になったら死亡処理
        }
    }

    // 敵が倒れた時の処理
    void Die()
    {
        Debug.Log("敵が倒れた！");
        
        Destroy(gameObject); // オブジェクトを削除
    }
}
