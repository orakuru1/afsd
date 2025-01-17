using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTG : MonoBehaviour
{
    public Transform player; //プレイヤーの位置
    public float speed = 3f; //敵の移動速度
    public float stoppingDistance = 2f; //停止距離
    public float detectionRange = 10f;  //検知範囲

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if(distance < detectionRange && distance > stoppingDistance)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        }
    }
}
