using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab; // 敵のPrefab
    public Transform spawnPoint;  // 敵の生成位置

    public Transform hpBarParent; // HPバーの親（Canvas）

    // Start is called before the first frame update

    public void SpawnEnemy()
    {
        // 敵を生成
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

        // HPバーの親を設定
        HealthBarManager healthBarManager = enemy.GetComponent<HealthBarManager>();
        if (healthBarManager != null)
        {
            //healthBarManager.hpBarParent = hpBarParent;
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
