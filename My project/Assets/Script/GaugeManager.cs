using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GaugeManager : MonoBehaviour
{
    public GameObject gaugeInstance; // HPバーのインスタンス
    private Slider hpSlider;
    private Transform characterTransform; // キャラクターのTransform
    public Player player; // プレイヤーキャラクターを参照

    [Header("ゲージ設定")]
    public float maxGauge = 100f; // ゲージの最大値
    //public float currentGauge = 0f; // 現在のゲージ値
    //public float fillRate = 10f; // ゲージの増加速度（1秒あたり）

    [Header("技の設定")]
    public string skillName = "必殺技"; // 技の名前
    public bool canUseSkill = false; // 技が使用可能かどうか

    [Header("ゲージリセットの設定")]
    public float skillGaugeCost = 100f; // 技発動時のゲージコスト

    void Start()
    {
        if (gaugeInstance != null)
        {
            hpSlider = gaugeInstance.GetComponentInChildren<Slider>(); //スライダーをいじれるようにした
            hpSlider.maxValue = maxGauge;
            hpSlider.value = player.currentGauge;
        }

        characterTransform = this.transform;

        if (player != null) //すでにたまってるゲージ分を最初に反映されるようにする
        {
            //UpdateHealth(player.currentHealth, player.maxHealth);
        }
    }

    void Update()
    {
        // キャラクターの位置に応じてHPバーの位置を更新
        /*
        if (gaugeInstance != null)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(characterTransform.position + Vector3.up * 4.0f); 
            gaugeInstance.transform.position = screenPosition;
        }
        */
        
        if (gaugeInstance != null)
        {
            // キャラクターの位置をスクリーン座標に変換
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 2.3f);

            // スクリーン座標がカメラの視野内にあるか確認
            if (screenPosition.z > 0) // Z座標が0以下だとカメラの背面になる
            {
                gaugeInstance.transform.position = screenPosition;
            }
            else
            {
                gaugeInstance.SetActive(false); // カメラ背面の場合は非表示
            }
        }

        // ゲージを自動で増加
        //FillGauge(Time.deltaTime * fillRate);

        // 技が使えるかどうかをチェック
        if (player.currentGauge >= maxGauge)
        {
            canUseSkill = true;
        }
    }

    // ゲージを増やす処理
    public void FillGauge(float amount)
    {
        player.currentGauge += amount;

        // ゲージを最大値に制限
        if (player.currentGauge > maxGauge)
        {
            player.currentGauge = maxGauge;
        }

        // SliderのUIに反映
        if (hpSlider != null)
        {
            hpSlider.value = player.currentGauge;
        }
    }

    // ゲージを消費して技を発動
    public void UseSkill()
    {
        if (canUseSkill)
        {
            Debug.Log($"{skillName} を発動！");
            player.currentGauge -= skillGaugeCost;

            // ゲージをリセット
            if (player.currentGauge < 0)
            {
                player.currentGauge = 0;
            }

            // 技の使用を無効化
            canUseSkill = false;

            // SliderのUIを更新
            if (hpSlider != null)
            {
                hpSlider.value = player.currentGauge;
            }
        }
        else
        {
            Debug.Log("ゲージが足りません！");
        }
    }
}
