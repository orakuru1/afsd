using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chara : MonoBehaviour
{
    [SerializeField] public bool onGround = true;
    [SerializeField] public bool inJumping = false;

    private Rigidbody rb;
    private Animator anim;

    [Header("Movement Settings")]
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private float sprintSpeed = 6.0f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 8.0f;
    [SerializeField] private float forwardJumpForce = 6.0f;
    [SerializeField] private float backwardJumpForce = 3.0f;

    private float moveSpeed;
    private bool isAttacking = false; // 攻撃状態を追跡
    private Transform cameraTransform; // カメラのTransformを取得

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        // メインカメラのTransformを取得
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        if (!isAttacking) // 攻撃中は他の動作をブロック
        {
            HandleMovement();
            HandleJump();
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

            anim.SetBool("walking", false);
        }
    }

    private void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            isAttacking = true;
            anim.SetTrigger("attack");

            StartCoroutine(ResetAttack());
        }
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(1.0f);
        isAttacking = false;
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            onGround = true;
            inJumping = false;
        }
    }
}
