using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponColliderController : MonoBehaviour
{
    public Collider weaponCollider; //武器にアタッチされたコライダー
    public float colliderActiveTime = 0.5f; //コライダーをオンにする時間

    private  bool isAttacking = false; //攻撃中かどうかのフラグ
    // Start is called before the first frame update
    void Start()
    {
        if(weaponCollider != null)
        {
            weaponCollider.enabled = false; //初期状態ではコライダーをオフにする
        }
    }

    //攻撃を開始するメソッド
    public void Attack()
    {
        if(!isAttacking)
        {
            StartCoroutine(ActivateCollider());
        }
    }

    //コライダーを一時的にオンにするコルーチン
    private System.Collections.IEnumerator ActivateCollider()
    {
        isAttacking = true; //攻撃中フラグを設定

        if(weaponCollider != null)
        {
            weaponCollider.enabled = true; //コライダーをオンにする
        }

        yield return new WaitForSeconds(colliderActiveTime); //指定時間待つ

        if(weaponCollider != null)
        {
            weaponCollider.enabled = false; //コライダーをオフに戻す
        }

        isAttacking = false; //攻撃終了
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
