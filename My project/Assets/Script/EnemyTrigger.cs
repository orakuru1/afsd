using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyTrigger : MonoBehaviour
{
    public string battleSceneName = "BattleScene"; // 戦闘シーンの名前
    public string enemyName = "Goblin"; // 敵の名前
    public int enemyHealth = 100; // 敵の体力
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // BattleDataに敵の情報を設定
            BattleData.Instance.SetEnemyData(this.gameObject, enemyName, enemyHealth);
            
            // 戦闘シーンに遷移
            SceneManager.LoadScene(battleSceneName);
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
