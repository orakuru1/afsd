using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{

    public GameObject hpBarInstance; // HPバーのインスタンス
    private Slider hpSlider;

    private Transform characterTransform; // キャラクターのTransform
    public Player player; // プレイヤーキャラクターを参照

    // Start is called before the first frame update
    void Start()
    {
        // HPバーのSliderを取得
        if (hpBarInstance != null)
        {
            hpSlider = hpBarInstance.GetComponentInChildren<Slider>(); //スライダーをいじれるようにした
        }

        // キャラクターのTransformを取得
        characterTransform = this.transform;
        
        // プレイヤーの初期HPをHPバーに反映
        if (player != null)
        {
            UpdateHealth(player.currentHealth, player.maxHealth);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // キャラクターの位置に応じてHPバーの位置を更新
        if (hpBarInstance != null && player != null)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(characterTransform.position + Vector3.up * 2.0f); 
            hpBarInstance.transform.position = screenPosition;
        }
        else
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(characterTransform.position + Vector3.up * 2.1f); 
            hpBarInstance.transform.position = screenPosition;
        }
        
        if (hpBarInstance != null && player != null)
        {
            // キャラクターの位置をスクリーン座標に変換
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 2.0f);

            // スクリーン座標がカメラの視野内にあるか確認
            if (screenPosition.z > 0) // Z座標が0以下だとカメラの背面になる
            {
                hpBarInstance.transform.position = screenPosition;
                hpBarInstance.SetActive(true);
            }
            else
            {
                hpBarInstance.SetActive(false); // カメラ背面の場合は非表示
            }
        }
        else
        {
            // キャラクターの位置をスクリーン座標に変換
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 2.2f);

            // スクリーン座標がカメラの視野内にあるか確認
            if (screenPosition.z > 0) // Z座標が0以下だとカメラの背面になる
            {
                hpBarInstance.transform.position = screenPosition;
                hpBarInstance.SetActive(true);
            }
            else
            {
                hpBarInstance.SetActive(false); // カメラ背面の場合は非表示
            }
        }

        if (hpSlider.value <= hpSlider.maxValue * 0.25f)
        {
            hpSlider.fillRect.GetComponent<Image>().color = Color.red;
        }
    }

    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        // HPバーの値を更新
        if (hpSlider != null)
        {
            Debug.Log($"{currentHealth}と{maxHealth}");
            hpSlider.value = currentHealth / maxHealth;
        }
    }

    private void OnDestroy()
    {
        // キャラクターが削除された際にHPバーも削除
        if (hpBarInstance != null)
        {
            Destroy(hpBarInstance);
        }
    }
}
