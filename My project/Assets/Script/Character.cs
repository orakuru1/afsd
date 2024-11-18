using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    // Start is called before the first frame update
    public int Charahealth = 50;
    public void CharaTakeDamage(int damage)
    {
        Debug.Log("aaaa");
        Charahealth -= damage;
        if (Charahealth <= 0)
        {
            Debug.Log("やられました");
            Die();
        }
    }
    private void Die()
    {
        // 敵が死亡する処理（例: エフェクトやスコアの増加など）
        Destroy(gameObject);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
