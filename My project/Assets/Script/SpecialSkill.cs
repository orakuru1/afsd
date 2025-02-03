using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpecialSkill : MonoBehaviour
{
    protected Player player;
    protected BattleManager battleManager;

    public void Initialize(Player player, BattleManager battleManager)
    {
        this.player = player;
        this.battleManager = battleManager;
    }

    // すべてのスペシャル技で共通の発動メソッド
    public void Activate()
    {
        StartCoroutine(PerformSkill());
    }

    protected void PushInSpecial() //押した後の処理
    {
        player.currentGauge = 0f; 
        player.GetComponent<GaugeManager>().FillGauge(player.currentGauge);
        battleManager.BuckSpecial();
    }

    // 派生クラスで実装する処理
    protected abstract IEnumerator PerformSkill();
}
