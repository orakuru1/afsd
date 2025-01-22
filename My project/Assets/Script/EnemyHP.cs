using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    public int maxHP = 100; // 敵の最大HP
    private int currentHP;

    public GameObject hpBarCanvas; //HPバーのCanvas
    public Image hpFillImage; //前景のImage(緑部分)
   
    void Start()
    {
        currentHP = maxHP; // 初期化
        hpBarCanvas.SetActive(false); //初期状態で非表示
    }

    // ダメージを受けるメソッド
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        currentHp = Mathf.Clamp(currentHP, 0, maxHP);

        //HPバーを更新
        UpdateHPBar();

        if (currentHP <= 0)
        {
            Die(); // HPが0以下になったら死亡処理
        }
    }

    void UpdateHPBar()
    {
        if(hpFillImage != null)
        {
            hpFillImage.fillAmount = (float)currentHP / maxHP;
        }
    }

    // 敵が倒れた時の処理
    void Die()
    {
        Debug.Log("敵が倒れた！");
        
        Destroy(gameObject); // オブジェクトを削除
    }
}
