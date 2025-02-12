using UnityEngine;
using UnityEngine.UI;

public class EnemyHP : MonoBehaviour
{
    public int maxHP = 100;    // 最大HP
    public int maxShield = 50; //最大シールド
    private int currentHP;
    private int currentShield;

    public Slider hpSlider; // HPバー用スライダー
    public Text hpText;     // HPを表示するテキスト

    public Slider shieldSlider;  //シールドバー用スライダー

    public float shieldDamageMultiplier = 1.1f; //シールドへのダメージ倍率
    public float smoothSpeed = 5f;  //スライダーの減少スピード

    void Start()
    {
        currentHP = maxHP; // 初期化
        currentShield = maxShield;

        UpdateHPUI(); // 初期状態のUI更新
    }

    void Update()
    {
        //スライダーをスムーズに更新
        if(shieldSlider != null)
        {
            shieldSlider.value = Mathf.Lerp(shieldSlider.value, (float) currentShield / maxShield, Time.deltaTime * smoothSpeed);
        }
        if(hpSlider != null)
        {
            hpSlider.value = Mathf.Lerp(hpSlider.value, (float)currentHP / maxHP, Time.deltaTime * smoothSpeed);
        }
    }

    // ダメージを受ける処理
    public void TakeDamage(int damage)
    {
        if(currentShield > 0)
        {
            //シールドは通常ダメージの1.3倍受ける
            int shieldDamage = Mathf.RoundToInt(damage * shieldDamageMultiplier);
            currentShield -= shieldDamage;

            //シールドがマイナスになった場合、余ったダメージをHPへ
            if(currentShield < 0)
            {
                currentHP += currentShield;  //currentShieldが負の値なので、その分だけHPを減らす
                currentShield = 0;
            }
        }
        else
        {
            currentHP -= damage;
        }

        currentHP = Mathf.Clamp(currentHP, 0, maxHP); //HPを0～maxHPに制限

        UpdateHPUI(); //UI更新

        if(currentHP <= 0)
        {
            Die();
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

        if(shieldSlider != null)
        {
            shieldSlider.value = (float)currentShield / maxShield;
        }
    }

    // 敵が倒れた時の処理
    void Die()
    {
        Debug.Log("敵が倒れた！");
        Destroy(gameObject); // 敵オブジェクトを削除
    }
}
