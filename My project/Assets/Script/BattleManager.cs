using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public Transform enemySpawnPoint; // 敵を生成する位置
    public Transform enemySpawnPoint2; // 敵を生成する位置
    public Transform enemySpawnPoint3; // 敵を生成する位置
    public Text enemyNameText;        // 敵の名前を表示するUI
    public Slider enemyHealthSlider;  // 敵の体力を表示するUI

    public Text battleLog;           // バトルログを表示するUI
    public Player player;            // プレイヤーのスクリプト
    public Enemy enemy;              // 敵のスクリプト
    private bool isPlayerTurn = true; // プレイヤーのターンかどうか

    public GameObject hpBarPrefab; // HPバーのPrefab
    public Transform hpBarParent; // HPバーの親（Canvas）

    private Enemy enemy3;
    private Enemy enemy4;

    //private Enemy enemy2;
    List<GameObject> II = new List<GameObject>(); //InstanceInformation
    
    void Start()
    {
        SetupBattle();
        StartBattle();

        // プレイヤーを探してHPバーを生成
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            CreateHealthBarFor(player);
        }
        else{
            Debug.Log("いません！");
        }

        // 敵を探してHPバーを生成（複数の敵に対応）
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            CreateHealthBarFor(enemy);
        }

    }

    void CreateHealthBarFor(GameObject character)
    {
        // HPバーを生成して親に設定
        GameObject hpBar = Instantiate(hpBarPrefab, hpBarParent);

        // キャラクターの位置に応じたHPバーを管理するスクリプトを設定
        HealthBarManager healthBarManager = character.AddComponent<HealthBarManager>();
        healthBarManager.hpBarInstance = hpBar;
    }

    public void ClearBattleLog()
    {
        battleLog.text = ""; // 空文字列でテキストをクリア
    }

    public void AddLog(string message)
    {
        battleLog.text += "\n" + message; // メッセージを追加
    }

    void StartBattle()
    {
        battleLog.text = "戦闘開始！";
        isPlayerTurn = true;
        UpdateBattleState();
    }

    void UpdateBattleState()
    {
        if (player.health <= 0)
        {
            EndBattle2(false); // プレイヤーの敗北
        }
        else if (enemy.health <= 0)
        {
            EndBattle2(true); // プレイヤーの勝利
        }
        else
        {
            if (isPlayerTurn)
            {
                battleLog.text += "\nプレイヤーのターン！";
                EnablePlayerActions(true);
            }
            else
            {
                battleLog.text += "\n敵のターン！";
                EnablePlayerActions(false);
                Invoke("EnemyTurn", 2); // 2秒後に敵のターンを実行
                
            }
        }
    }

    public void PlayerAttack(int damage)
    {
        if (!isPlayerTurn) return;

        //int damage = Random.Range(5, 15);
        ClearBattleLog();
        battleLog.text += $"\nプレイヤーが敵に{damage}のダメージ！";
        //enemy.TakeDamage(damage);

        isPlayerTurn = false;
        UpdateBattleState();
/*
        enemy2 = FindObjectOfType<Enemy>();  ///違うやつが探されそう
        if (enemy != null)
        {
            battleManager = FindObjectOfType<BattleManager>();
            battleManager.PlayerAttack();
            //enemy.TakeDamage(damage);
        }
*/
    }

    void EnemyTurn()        //敵のターンが発動できない
    {
        int damage = Random.Range(5, 15);
        ClearBattleLog();
        battleLog.text += $"\n敵がプレイヤーに{damage}のダメージ！";
        player.TakeDamage(damage);

        isPlayerTurn = true;
        UpdateBattleState();
    }

    void EnablePlayerActions(bool enable)
    {
        // プレイヤーの行動UI（ボタンなど）を有効化または無効化
        // ここでは例として簡単に設定
        Button attackButton = GameObject.Find("AttackButton").GetComponent<Button>();
        attackButton.interactable = enable;
    }

    void EndBattle2(bool isPlayerWin)
    {
        if (isPlayerWin)
        {
            battleLog.text += "\nプレイヤーの勝利！";
        }
        else
        {
            battleLog.text += "\nプレイヤーの敗北...";
        }
    }

    void SetupBattle()  
    {
        // BattleDataから敵の情報を取得して設定
        if (BattleData.Instance.enemyName != null)
        {
            // 敵のPrefabを生成
            GameObject prefab = (GameObject)Resources.Load (BattleData.Instance.enemyName);
            II.Add(Instantiate(prefab, enemySpawnPoint.position, Quaternion.identity));
            II.Add(Instantiate(prefab, enemySpawnPoint2.position, Quaternion.identity));
            II.Add(Instantiate(prefab, enemySpawnPoint3.position, Quaternion.identity));

            enemy3 = II[0].GetComponent<Enemy>(); //生成された敵オブジェクトのscript<Enemy>を習得
            enemy4 = II[1].GetComponent<Enemy>();
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
