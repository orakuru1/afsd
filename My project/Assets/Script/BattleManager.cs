using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public Transform enemySpawnPoint; // 敵を生成する位置
    public Text enemyNameText;        // 敵の名前を表示するUI
    public Slider enemyHealthSlider;  // 敵の体力を表示するUI

    private GameObject enemyInstance;
    
    void Start()
    {
        SetupBattle();
    }

    void SetupBattle()   //現在てきのオブジェクト情報持ってこれず
    {
        // BattleDataから敵の情報を取得して設定
        if (BattleData.Instance.enemyName != null)
        {
            // 敵のPrefabを生成
            GameObject prefab = (GameObject)Resources.Load (BattleData.Instance.enemyName);
            enemyInstance = Instantiate(prefab, enemySpawnPoint.position, Quaternion.identity);
            //Debug.Log(BattleData.Instance.enemyPrefab);

            /*
            // UIに敵の情報を反映
            enemyNameText.text = BattleData.Instance.enemyName;
            enemyHealthSlider.maxValue = BattleData.Instance.enemyHealth;
            enemyHealthSlider.value = BattleData.Instance.enemyHealth;
            */
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
