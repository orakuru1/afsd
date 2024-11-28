using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chara : MonoBehaviour
{
    [SerializeField] public bool onGround = true;
    [SerializeField] public bool inJumping = false;
    Rigidbody rb; //rigidbody定義
    float speed = 3.0f; //移動スピード
    float sprintspeed = 6.0f; //ダッシュ
    float angleSpeed = 150;
    float v;
    float h;
    //Start is called before the first frame update
    void Start()
    {
       rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //wキー（前方移動）
        if(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
        {
            v = Time.deltaTime * sprintspeed;
        }

        //sキー（後方移動）
        else if(Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.LeftShift))
        {
            v = -Time.deltaTime * sprintspeed;
        }

        //Dキー（右移動）
        else if(Input.GetKey(KeyCode.W))
        {
            v = Time.deltaTime * speed;
        }

        //Aキー（左移動）
        else if(Input.GetKey(KeyCode.S))
        {
            v = -Time.deltaTime * speed;
        }
        else 
        {
            v = 0;
        }
       

        //移動の実行
        if(!inJumping)
        {
            transform.position += transform.forward * v;
        }

        //スペースボタンでジャンプ
        if(onGround)
        {
            if(Input.GetKey(KeyCode.Space))
            {
                //ジャンプさせるため上方向に力を発生
                rb.AddForce(transform.up * 8, ForceMode.Impulse);
                //ジャンプ中は地面との接触判定をfalseにする
                onGround = false;
                inJumping = true;

                //前後キーを押しながらジャンプしたときは前後方向も力を発生させる
                if(Input.GetKey(KeyCode.W))
                {
                    rb.AddForce(transform.forward * 6f,ForceMode.Impulse);
                }
                else if(Input.GetKey(KeyCode.S))
                {
                    rb.AddForce(transform.forward * -3f, ForceMode.Impulse);
                }
            }
        }

        //左右キーで方向転換
       if(Input.GetKey(KeyCode.RightArrow))
        {
            h = Time.deltaTime * angleSpeed;
        }
        else if(Input.GetKey(KeyCode.LeftArrow))
        {
            h = -Time.deltaTime * angleSpeed;
        }
        else 
        {
            h = 0;
        }

        transform.Rotate(Vector3.up * h);
    }

    //地面に接触したときonGroundをtrue inJumpingをfalseにする
    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Ground")
        {
            onGround = true;
            inJumping = false;
        }

    }
}
