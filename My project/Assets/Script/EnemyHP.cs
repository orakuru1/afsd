using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyHP : MonoBehaviour
{
    public int maxHP = 100;    // 最大HP
    public int maxShield = 50; //最大シールド
    private int currentHP;
    private int currentShield;

    public Slider hpSlider; // HPバー用スライダー
    public Slider hpDelayedSlider; //遅延して減るHPバー
    public Text hpText;     // HPを表示するテキスト

    public Slider shieldSlider;  //シールドバー用スライダー
    public Slider shieldDelayedSlider; //遅延シールドバー

    public float shieldDamageMultiplier = 1.1f; //シールドへのダメージ倍率
    public float smoothSpeed = 3f;  //スライダーの減少スピード
    public float delayedSpeed = 5f; //遅延バーの減少スピード

    private float targetHP;      //HPスライダーの目標値
    private float targetShield;  //シールドスライダーの目標値
    private bool isStunned = false; //行動不能フラグ

    private EnemyRangedAttack enemyAI;  //敵のAI
    private EnemyTG enemyai;

    public DefeatTextManager defeatTextManager;

    void Start()
    {
        currentHP = maxHP; // 初期化
        currentShield = maxShield;
        targetHP = maxHP;
        targetShield = maxShield;

        enemyAI = GetComponent<EnemyRangedAttack>();
        enemyai = GetComponent<EnemyTG>();

        UpdateHPUI(); // 初期状態のUI更新

       
    }

    void Update()
    {
        //HPバーの更新
        if(hpSlider != null)
        {
            hpSlider.value = Mathf.Lerp(hpSlider.value, (float)currentHP / maxHP, Time.deltaTime * smoothSpeed);
        }

        //HP遅延バー
        if(hpDelayedSlider != null)
        {
            hpDelayedSlider.value = Mathf.Lerp(hpDelayedSlider.value, hpSlider.value, Time.deltaTime * delayedSpeed);
        }

        //シールドバーの更新
        if(shieldSlider != null)
        {
            shieldSlider.value = Mathf.Lerp(shieldSlider.value, (float) currentShield / maxShield, Time.deltaTime * smoothSpeed);
        }

        if(shieldDelayedSlider != null)
        {
            shieldDelayedSlider.value = Mathf.Lerp(shieldDelayedSlider.value, shieldSlider.value, Time.deltaTime * delayedSpeed);
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
                Debug.Log("シールドが0になりました");
                currentHP += currentShield;  //currentShieldが負の値なので、その分だけHPを減らす
                currentShield = 0;

                //シールドが0になったら5秒間行動不能になる
                if(!isStunned)
                {
                    StartCoroutine(StunEnemy(5f));
                }
            }
        }
        else
        {
            currentHP -= damage;
        }

        currentHP = Mathf.Clamp(currentHP, 0, maxHP); //HPを0～maxHPに制限

        //目標値を更新
        targetHP = currentHP;
        targetShield = currentShield;

        UpdateHPUI(); //UI更新

        if(currentHP <= 0)
        {
            Die();
        }
    }

    //シールドが0になったら敵の行動を5秒間止める
    IEnumerator StunEnemy(float duration)
    {
        isStunned = true;

        if(enemyAI != null)
        {
            enemyAI.enabled = false; //敵の行動を停止
        }

        if(enemyai != null)
        {
            enemyai.enabled = false;
        }
        Debug.Log("敵が5秒間行動不能");

        yield return new WaitForSeconds(duration);

        if(enemyAI != null)
        {
            enemyAI.enabled = true; //敵の行動を再開
        }

        if(enemyai != null)
        {
            enemyai.enabled = true;
        }

        Debug.Log("敵が動いた");
        isStunned = false;
    }

    // HPバーとテキストの更新
    void UpdateHPUI()
    {
    
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
        if(defeatTextManager != null)
        {
            defeatTextManager.ShowDefeatText();
        }
    }
}
