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

    void Start()
    {
        if (gaugeInstance != null)
        {
            hpSlider = gaugeInstance.GetComponentInChildren<Slider>(); //スライダーをいじれるようにした
            hpSlider.maxValue = player.maxGauge;
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
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 2.1f);

            // スクリーン座標がカメラの視野内にあるか確認
            if (screenPosition.z > 0) // Z座標が0以下だとカメラの背面になる
            {
                gaugeInstance.transform.position = screenPosition;
                gaugeInstance.SetActive(true);
            }
            else
            {
                gaugeInstance.SetActive(false); // カメラ背面の場合は非表示
            }
        }

        // ゲージを自動で増加
        //FillGauge(Time.deltaTime * fillRate);
    }

    // ゲージを増やす処理
    public void FillGauge(float amount)
    {
        player.currentGauge += amount;

        // ゲージを最大値に制限
        if (player.currentGauge > player.maxGauge)
        {
            player.currentGauge = player.maxGauge;
        }

        // SliderのUIに反映
        if (hpSlider != null)
        {
            hpSlider.value = player.currentGauge;
            Debug.Log(hpSlider.value);
        }
    }
    private void OnDestroy()
    {
        // キャラクターが削除された際にHPバーも削除
        if (gaugeInstance != null)
        {
            Destroy(gaugeInstance);
        }
    }
}
