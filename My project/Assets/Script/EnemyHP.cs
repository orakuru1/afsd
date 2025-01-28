using UnityEngine;
using UnityEngine.UI;

public class EnemyHP : MonoBehaviour
{
    public int maxHP = 100; // 最大HP
    private int currentHP;

    public Slider hpSlider; // HPバー用スライダー
    public Text hpText; // HPを表示するテキスト

    void Start()
    {
        currentHP = maxHP; // 初期化
        UpdateHPUI(); // 初期状態のUI更新
    }

    // ダメージを受ける処理
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP); // HPを0～maxHPに制限
      

        UpdateHPUI(); // UIを更新

        if (currentHP <= 0)
        {
            Die(); // HPが0になったら死亡処理
        }
    }

    // HPバーとテキストの更新
    void UpdateHPUI()
    {
        if (hpSlider != null)
        {
            hpSlider.value = (float)currentHP / maxHP; // HPスライダーの値を更新
        }

        if (hpText != null)
        {
            hpText.text = $"{currentHP} / {maxHP}"; // HPテキストの更新
        }
    }

    // 敵が倒れた時の処理
    void Die()
    {
        Debug.Log("敵が倒れた！");
        Destroy(gameObject); // 敵オブジェクトを削除
    }
}
