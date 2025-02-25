using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{

    public GameObject hpBarInstance; // HPバーのインスタンス
    
    private Slider hpSlider;

    public Player player; // プレイヤーキャラクターを参照

    private Vector3 worldPosition = new Vector3();

    private Collider Characollider;

    private float objectHeight;

    // Start is called before the first frame update
    void Start()
    {
        // HPバーのSliderを取得
        if (hpBarInstance != null)
        {
            hpSlider = hpBarInstance.GetComponentInChildren<Slider>(); //スライダーをいじれるようにした
        }

        // キャラクターのTransformを取得
        
        // プレイヤーの初期HPをHPバーに反映
        if (player != null)
        {
            UpdateHealth(player.currentHealth, player.maxHealth);
        }

        if (hpBarInstance != null && player == null)
        {
            hpBarInstance.transform.localScale = new Vector3(0.07f,0.1f,0f);
        }

        Characollider = GetComponent<Collider>();
        objectHeight = Characollider.bounds.size.y;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // キャラクターの位置に応じてHPバーの位置を更新
        if (hpBarInstance != null && player != null)
        {
            worldPosition = new Vector3(0f, objectHeight + 0.1f,0f) + transform.position;
        }
        else
        {
            worldPosition = new Vector3(0.1f, objectHeight + 0.1f,0f) + transform.position;
        }

        hpBarInstance.transform.position = worldPosition;



        if (hpSlider.value <= hpSlider.maxValue * 0.25f)//HPが２５％以下なら赤色にする
        {
            hpSlider.fillRect.GetComponent<Image>().color = Color.red;
        }
        else
        {
            hpSlider.fillRect.GetComponent<Image>().color = Color.green; // 通常は緑
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
