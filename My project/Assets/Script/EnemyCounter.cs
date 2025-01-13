using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCounter : MonoBehaviour
{
    //public Text enemyCountText; // UIのTextコンポーネント
    private int enemyCount = 0;    

    private BattleManager battleManager;

    void Start()
    {
        //UpdateEnemyCount();
    }

    // 敵が生成されたときに呼ばれる
    public void OnEnemySpawn()
    {
        enemyCount++;
        UpdateEnemyCount();
    }

    // 敵が破壊されたときに呼ばれる
    public void OnEnemyDestroyed()
    {
        enemyCount--;
        UpdateEnemyCount();
    }

    private void UpdateEnemyCount()
    {
        /*
        if (enemyCountText != null)
        {
            enemyCountText.text = "敵の数: " + enemyCount;
        }
        */

        battleManager = FindObjectOfType<BattleManager>();
        if (enemyCount <= 0)
        {
            battleManager.EndBattle();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
