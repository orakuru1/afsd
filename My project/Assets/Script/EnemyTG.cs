using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTG : MonoBehaviour
{
    public Transform player; //プレイヤーの位置
    public float speed = 3f; //敵の移動速度
    public float stoppingDistance = 2f; //停止距離
    public float detectionRange = 10f;  //検知範囲

    private Animator anim;

    public GameObject hpBarCanvas; //HPバーのCanvas
    public Image hpFillImage; //前景のImage(緑部分)
     

    

    // Start is called before the first frame update
    public void aaaa()
    {
       //プレイヤーをタグで自動取得
        GameObject playerObject = GameObject.FindWithTag("Player");
        if(playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.Log("見つかりません");
        }
    }
    
    void Start()
    {
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if(player == null)
        {
            if(hpBarCanvas != null)
            {
                hpBarCanvas.SetActive(false);
            }
            return;
        }

        //プレイヤーとの距離を計算
        float distance = Vector3.Distance(transform.position, player.position);

        if(distance < detectionRange && distance > stoppingDistance)
        {
            //プレイヤーに向かって移動
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            //歩くアニメーション
            anim.SetBool("walking", true);
            //攻撃アニメーション
            anim.SetBool("attack", true);
            //プレイヤーの方向を向く
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        }
        else
        {
            //歩くアニメーションを停止
            anim.SetBool("walking",false);
            //攻撃アニメーションを停止
            anim.SetBool("attack", false);
        }
    }
}
