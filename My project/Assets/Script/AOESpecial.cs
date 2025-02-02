using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOESpecial : SpecialSkill
{
    public override void kari() //こっちで変えれるのかのお試し
    {
        Debug.Log("子供だよー");
        base.kari();//これをすると変更しても、問題なく親を呼び出せれる
    }
    protected override IEnumerator PerformSkill()
    {
        battleManager.AddLog($"{player.name} の範囲攻撃発動！");
        battleManager.BuckSpecial();

        yield return new WaitForSeconds(2f);

        foreach (Enemy enemy in BattleManager.enemys)
        {
            enemy.TakeDamage((player.attack * 2 + player.weapon[0].number), player);
            EnemyDestroyGuage eneguage = enemy.GetComponent<EnemyDestroyGuage>();
            eneguage.FillGauge(player.sharp*2);
        }

        player.currentGauge = 0f; // ゲージをリセット
        battleManager.BuckSpecial();
        battleManager.ClearBattleLog();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
