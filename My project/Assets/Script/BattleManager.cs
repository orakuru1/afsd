using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    void SetupBattle()  
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

    public void EndBattle()
    {
        // BattleDataの情報をリセット（必要に応じて）
        BattleData.Instance.SetEnemyData( "", 0);

        // フィールドシーンに戻る
        SceneManager.LoadScene("SampleScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
