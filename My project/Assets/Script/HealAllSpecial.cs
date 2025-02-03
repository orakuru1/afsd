using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAllSpecial : SpecialSkill
{
    protected override IEnumerator PerformSkill()
    {
        battleManager.AddLog($"{player.name} の全体回復発動！");
        battleManager.BuckSpecial();

        yield return new WaitForSeconds(2f);
        battleManager.ClearBattleLog();

        foreach (Player ally in BattleManager.players)
        {
            float healAmount = player.attack * 1.5f;
            ally.Heal(healAmount);
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
