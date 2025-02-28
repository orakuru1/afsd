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

    private Vector3 worldPosition = new Vector3();

    private Collider Characollider;

    private float objectHeight;

    void Start()
    {
        enemy = GetComponent<Enemy>();
        if(Guage != null)
        {
            slider = Guage.GetComponentInChildren<Slider>(); //スライダーをいじれるようにした
            slider.maxValue = maxGauge;
            slider.value = currentGauge;

        }

        Characollider = GetComponent<Collider>();
        objectHeight = Characollider.bounds.size.y;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Guage != null)
        {
            worldPosition = new Vector3(0.1f, objectHeight + 0.3f,0f) + transform.position;
        }

        Guage.transform.position = worldPosition;

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
            enemy.isBurst = true;
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
