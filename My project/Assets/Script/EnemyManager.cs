using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static List<Enemy> enemies = new List<Enemy>(); // 敵リスト
    void Start()
    {
        // シーン内のすべての敵をリストに追加
        foreach (GameObject enemyObj in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Enemy enemy = enemyObj.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemies.Add(enemy);
            }
        }
    }

    //一番左にいる敵を習得するメソッド
    public static Enemy GetLeftMostEnemy()
    {
        if(enemies.Count == 0) return null;

        //x座標が一番小さい敵を特定
        Enemy leftMost = enemies[0];
        foreach(Enemy enemy in enemies)
        {
            if(enemy.transform.position.x < leftMost.transform.position.x)
            {
                leftMost = enemy;
            }
        }
        return leftMost;
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
