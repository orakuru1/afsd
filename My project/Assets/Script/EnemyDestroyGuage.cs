using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyDestroyGuage : MonoBehaviour
{
    [SerializeField]private GameObject Guage;
    [SerializeField]private float currentGauge = 0f;
    [SerializeField]private float maxGauge = 100f;
    [SerializeField]private Slider slider;
    private Enemy enemy;

    void Start()
    {
        enemy = GetComponent<Enemy>();
        if(Guage != null)
        {
            slider = Guage.GetComponentInChildren<Slider>(); //スライダーをいじれるようにした
            slider.maxValue = maxGauge;
            slider.value = currentGauge;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Guage != null)
        {
            // キャラクターの位置をスクリーン座標に変換
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 2.2f);

            // スクリーン座標がカメラの視野内にあるか確認
            if (screenPosition.z > 0) // Z座標が0以下だとカメラの背面になる
            {
                Guage.transform.position = screenPosition;
            }
            else
            {
                Guage.SetActive(false); // カメラ背面の場合は非表示
            }
        }

    }
    public void FillGauge(float amount)
    {
        currentGauge += amount;

        // ゲージを最大値に制限
        if (currentGauge > maxGauge)
        {
            currentGauge = maxGauge;
        }
        if(currentGauge >= maxGauge)
        {
            enemy.ChangeBurst();
        }

        // SliderのUIに反映
        if (slider != null)
        {
            slider.value = currentGauge;
        }
    }
    public void SetGuage()
    {
        currentGauge = 0f;
        slider.value = currentGauge;
    }
    public void SetObject(GameObject gameObject)
    {
        Guage = gameObject;
    }
    private void OnDestroy()
    {
        // キャラクターが削除された際にHPバーも削除
        if (Guage != null)
        {
            Destroy(Guage);
        }
    }
}
