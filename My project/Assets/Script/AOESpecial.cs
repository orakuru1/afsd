using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOESpecial : SpecialSkill
{
    protected override IEnumerator PerformSkill()
    {
        battleManager.AddLog($"{player.name} の範囲攻撃発動！");
        battleManager.BuckSpecial();

        yield return new WaitForSeconds(2f);
        battleManager.ClearBattleLog();

        foreach (Enemy enemy in BattleManager.enemys)
        {
            enemy.SupecialDamage((player.attack * 2 + player.weapon[0].number), player);
            EnemyDestroyGuage eneguage = enemy.GetComponent<EnemyDestroyGuage>();
            eneguage.FillGauge(player.sharp*2);
        }

        PushInSpecial();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
