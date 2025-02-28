using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GaugeManager : MonoBehaviour
{
    public GameObject gaugeInstance; // HPバーのインスタンス

    private Slider hpSlider;
    public Slider PAUS;

    public Text SPText;

    public Player player; // プレイヤーキャラクターを参照

    private Animator GBA;

    private Vector3 worldPosition = new Vector3();

    private Collider Characollider;

    private float objectHeight;

    void Start()
    {
        if (gaugeInstance != null)
        {
            hpSlider = gaugeInstance.GetComponentInChildren<Slider>(); //スライダーをいじれるようにした
            hpSlider.maxValue = player.maxGauge;
            hpSlider.value = player.currentGauge;
            PAUS.maxValue = player.maxGauge;
            PAUS.value = player.currentGauge;
            SPText.text = player.currentGauge.ToString();
        }

        Characollider = GetComponent<Collider>();
        objectHeight = Characollider.bounds.size.y;

    }

    void LateUpdate()
    {

        if (gaugeInstance != null)
        {
            worldPosition = new Vector3(0f, objectHeight + 0.3f, 0f) + transform.position;
        }

        gaugeInstance.transform.position = worldPosition;


        // ゲージを自動で増加
        //FillGauge(Time.deltaTime * fillRate);
    }

    public void GBASet(Animator animator)
    {
        GBA = animator;
    }

    // ゲージを増やす処理
    public void FillGauge(float amount)
    {
        player.currentGauge += amount;

        // ゲージを最大値に制限
        if (player.currentGauge >= player.maxGauge)
        {
            player.currentGauge = player.maxGauge;
        }

        // SliderのUIに反映
        if (hpSlider != null)
        {
            PAUS.value = player.currentGauge;
            hpSlider.value = player.currentGauge;
            SPText.text = player.currentGauge.ToString();
            Debug.Log(hpSlider.value);
        }
    }
    public IEnumerator Animation()
    {

        if (player.currentGauge >= player.maxGauge)
        {
            yield return new WaitForSeconds(0.5f);
            if(GBA != null)
            {
                GBA.SetBool("SpecialBool", true);
            }

        }
        else
        {
            GBA.SetBool("SpecialBool", false);
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
