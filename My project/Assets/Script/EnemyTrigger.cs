using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyTrigger : MonoBehaviour
{

    [SerializeField]private string enemyName = "Goblin"; // 敵の名前
    private int enemyHealth = 100; // 敵の体力
    private bool isstart = true;
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        if(isstart != true)
        {
            if (collision.collider.CompareTag("Player"))
            {
                // BattleDataに敵の情報を設定
                BattleData.Instance.SetEnemyData(enemyName, enemyHealth);

                BattleData.Instance.RePosition(collision.collider.transform.position);

                // 戦闘シーンに遷移
                //SceneManager.LoadScene(battleSceneName);
                StartCoroutine(BattleData.Instance.LoadBattleScene()); // 非同期ロード
            }
        }

    }

    public void TriggerStart()
    {
        isstart = false;
    }
    
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
