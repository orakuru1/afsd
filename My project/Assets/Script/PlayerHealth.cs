using UnityEngine;
using UnityEngine.UI; // UIを操作するために必要
using System.Collections;

public class PlayerHelth : MonoBehaviour
{
    public int maxHealth = 100; // 最大HP
    private int currentHealth; // 現在のHP
    public bool isDead{ get; private set;} = false; //死亡フラグ

    public Slider healthSlider; // HPバー
    public Text healthText; // HPのテキスト表示（オプション）
    public float smoothSpeed = 0.5f; // HPバーが減る速度（小さいほど遅い）

    private float targetSliderValue; // スライダーの目標値

    private Animator anim; //アニメーション

    void Start()
    {
        currentHealth = maxHealth; // 開始時のHP
        targetSliderValue = 1f; // スライダーは満タン（1.0）
        UpdateHealthUI(true); // HPのUIを初期化
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // HPを範囲内に制限

        // スライダーの目標値を計算
        targetSliderValue = (float)currentHealth / maxHealth;

        // UI更新をアニメーションで処理
        StartCoroutine(SmoothHealthBarChange());

        if (healthText != null)
        {
            healthText.text = currentHealth.ToString(); // HPテキストを更新
        }

        if (currentHealth <= 0)
        {
           PlayerDeath(); //HPが0以下なら死亡処理
        }
    }

    IEnumerator SmoothHealthBarChange()
    {
        while (Mathf.Abs(healthSlider.value - targetSliderValue) > 0.01f)
        {
            healthSlider.value = Mathf.Lerp(healthSlider.value, targetSliderValue, smoothSpeed);
            yield return null; // フレームを待つ
        }

        // 最後に値を正確に設定（誤差補正）
        healthSlider.value = targetSliderValue;
    }

    void UpdateHealthUI(bool instant = false)
    {
        if (healthSlider != null)
        {
            if (instant)
            {
                healthSlider.value = (float)currentHealth / maxHealth; // 即座に更新
            }
        }

        if (healthText != null)
        {
            healthText.text = currentHealth.ToString(); // HPテキストを更新
        }
    }

    void PlayerDeath()
    {
        isDead = true; //死亡フラグをオンにする
        anim.SetBool("die", true);
    }
}

