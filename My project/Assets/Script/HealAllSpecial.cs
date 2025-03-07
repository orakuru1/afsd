using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAllSpecial : SpecialSkill
{
    public ParticleSystem Particle;

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
            Instantiate(Particle, ally.transform);
        }

        PushInSpecial();
    }
    
    void Start()
    {
        ParticleSystem heal = Resources.Load<ParticleSystem>("Particle_Heal");

        if(heal != null)
        {
            Particle = heal;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
