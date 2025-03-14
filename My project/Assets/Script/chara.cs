using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chara : MonoBehaviour
{
    [SerializeField] public bool onGround = true;
    [SerializeField] public bool inJumping = false;
    static public bool run = false;
    private Rigidbody rb;
    private Animator anim;

    [Header("Movement Settings")]
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private float sprintSpeed = 6.0f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 20.0f;
    [SerializeField] private float forwardJumpForce = 8.0f;
    [SerializeField] private float backwardJumpForce = 3.0f;

    private float moveSpeed;
    private bool isAttacking = false; // 攻撃状態を追跡
    private Transform cameraTransform; // カメラのTransformを取得

    public ParticleSystem prepar, attack;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        // メインカメラのTransformを取得
        cameraTransform = Camera.main.transform;
        run = false;
    }

    void Update()
    {
        if (!isAttacking) // 攻撃中は他の動作をブロック
        {
            HandleMovement();
            HandleJump();
            if (!run)
            {
                RunEffect();
            }
        }
        HandleAttack(); // 攻撃処理は常に確認
    }

    private void HandleMovement()
    {
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        bool isMoving = false;
        Vector3 moveDirection = Vector3.zero;

        // カメラの向きを基準に移動方向を決定
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;
        cameraForward.y = 0; // 水平方向のみ考慮
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        if (Input.GetKey(KeyCode.W))
        {
            moveDirection += cameraForward; // カメラの前方向に移動
            isMoving = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDirection -= cameraForward; // カメラの後ろ方向に移動
            isMoving = true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection -= cameraRight; // カメラの左方向に移動
            isMoving = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDirection += cameraRight; // カメラの右方向に移動
            isMoving = true;
        }

        // 移動速度の決定
        moveSpeed = isSprinting ? sprintSpeed : speed;
        if (isMoving && !inJumping)
        {
            transform.position += moveDirection.normalized * moveSpeed * Time.deltaTime;
        }

        // プレイヤーの向きをカメラの向きと一致させる
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
        
        
        // アニメーション設定
        anim.SetBool("walking", isMoving);
        anim.SetBool("dassyu", isSprinting);
    }

    private void HandleJump()
    {
        if (onGround && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            onGround = false;
            inJumping = true;

            //プレイヤーの向いている方向に前進する力を加える
            if(Input.GetKey(KeyCode.W))
            {
                Vector3 forwardJump = transform.forward * forwardJumpForce;
                rb.AddForce(forwardJump, ForceMode.Impulse);
            }
            else if(Input.GetKey(KeyCode.S))
            {
                Vector3 forwardJump = transform.forward * forwardJumpForce;
                rb.AddForce(forwardJump, ForceMode.Impulse);
            }
            else if(Input.GetKey(KeyCode.A))
            {
                Vector3 forwardJump = transform.forward * forwardJumpForce;
                rb.AddForce(forwardJump, ForceMode.Impulse);
            }
            else if(Input.GetKey(KeyCode.D))
            {
                Vector3 forwardJump = transform.forward * forwardJumpForce;
                rb.AddForce(forwardJump, ForceMode.Impulse);
            }
            

            anim.SetBool("walking", false);
        }
    }

    private void HandleAttack()
    {
        // キャラクターの位置を取得
        Vector3 characterPosition = this.gameObject.transform.position;
        Vector3 characterForward = this.gameObject.transform.forward;
        float heightOffset = 3f;
        // 高さを加えた新しい位置
        Vector3 spawnPosition = characterPosition + new Vector3(0, heightOffset, 0);
        string player = GetComponent<Player>().pn;
        if (Input.GetMouseButtonDown(0) )
        {
            
            anim.SetBool("attack",true);
            if(player == "mahou")
            {
                Instantiate(attack, spawnPosition, Quaternion.LookRotation(characterForward));
            }
            StartCoroutine(ResetAttack());
            

            // 親を設定しない場合、そのまま親なしでインスタンス化されたオブジェクトはシーンに追加されます

        }


    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(0.01f);
        isAttacking = false;
        anim.SetBool("attack",false);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            onGround = true;
            inJumping = false;
        }
    }
    
    private void RunEffect()
    {
        if (!run && Input.GetKey(KeyCode.W) ||Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.S)||Input.GetKey(KeyCode.D))
        {
            Instantiate(prepar, this.gameObject.transform);
            run = true;
        }
    }
}
