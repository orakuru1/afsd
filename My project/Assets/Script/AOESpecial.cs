using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOESpecial : SpecialSkill
{
    public ParticleSystem particle;
    protected override IEnumerator PerformSkill()
    {
        battleManager.AddLog($"{player.name} の範囲攻撃発動！");
        battleManager.BuckSpecial();
        foreach (Enemy enemy in BattleManager.enemys)
        {
            Instantiate(particle, enemy.transform);
        }
            
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
        ParticleSystem attack = Resources.Load<ParticleSystem>("Particle_kirituke2");

        if(attack != null)
        {
            particle = attack;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
