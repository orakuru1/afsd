using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int attackDamage = 10; // 基本ダメージ

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))  //左クリックで攻撃
        {
            Attack();
        }
    }

    void Attack()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, 5f))
        {
            EnemyHP enemy = hit.collider.GetComponent<EnemyHP>();
            if(enemy != null)
            {
                enemy.TakeDamage(attackDamage);
            }
        }
    }
}
