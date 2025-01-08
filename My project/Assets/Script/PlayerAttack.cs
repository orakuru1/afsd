using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAttack : MonoBehaviour
{
    private BattleManager battleManager;
    [SerializeField] public Player player;            // プレイヤーのスクリプト
    public void Attack()
    {
        // 選択された敵を攻撃
        Enemy targetEnemy = Enemy.selectedEnemy;

        if (targetEnemy == null)
        {
            // 敵が選択されていない場合は、一番左の敵を攻撃
            targetEnemy = EnemyManager.GetLeftMostEnemy();
        }

        if (targetEnemy != null)
        {
            Debug.Log($"攻撃！ {targetEnemy.gameObject.name} を攻撃します。");
            ExecuteAttack(targetEnemy);
        }
        else
        {
            Debug.Log("攻撃する敵がいません！");
        }

        // 攻撃後、選択状態をリセット
        //Enemy.selectedEnemy = null;
    }

    private void ExecuteAttack(Enemy target)
    {
        // 攻撃処理（例: 敵にダメージを与える）
        target.GetComponent<Enemy>()?.TakeDamage(25); //１０の部分は攻撃力の参照にできそう
        //battleManager = FindObjectOfType<BattleManager>();
        //battleManager.PlayerAttack(25);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
