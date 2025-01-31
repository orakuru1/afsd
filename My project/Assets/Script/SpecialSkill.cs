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
    public virtual void kari() //virtualのお試し
    {
        Debug.Log("親元だよー");
    }

    // 派生クラスで実装する処理
    protected abstract IEnumerator PerformSkill();
}
