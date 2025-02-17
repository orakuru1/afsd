using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playersccan : MonoBehaviour
{
    public Transform player; //プレイヤーの位置
    public float speed = 3f; //敵の移動速度
    public float stoppingDistance = 2f; //停止距離
    public float detectionRange = 10f; // 検知範囲


    private Animator animator; // Animatorコンポーネント
    // Start is called before the first frame update
    void Start()
    {
        //Animatorコンポーネント取得
        animator = GetComponent<Animator>();

        //プレイヤーをタグで探す
        FindPlayer(); //初回プレイヤー検索
    }

    // Update is called once per frame
    void Update()
    {
        //毎フレームプレイヤーを再検索
        if(player == null || player.gameObject.tag != "Player")
        {
            FindPlayer();
        }
        if(player == null)
        {
            return; // プレイヤーがいない場合、処理を中断
        }

        //プレイヤーとの距離を計算
        float distance = Vector3.Distance(transform.position, player.position);

        //プレイヤーが検知範囲内かつ停止距離以上の場合
        if(distance < detectionRange && distance > stoppingDistance)
        {
            //歩くアニメーションを再生
            animator.SetBool("walking", true);

            //プレイヤーに向かって移動
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            //プレイヤーの方向を向く
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));  
        }
        else{
            //歩くアニメーションを停止
            animator.SetBool("walking", false);
        }
    }

    //プレイヤーを探すメソッド
    private void FindPlayer()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if(playerObj != null)
        {
            player = playerObj.transform;
        }
    }
}
