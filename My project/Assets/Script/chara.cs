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
    [SerializeField] private float rotationSpeed = 150.0f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 8.0f;
    [SerializeField] private float forwardJumpForce = 6.0f;
    [SerializeField] private float backwardJumpForce = 3.0f;

    private float moveSpeed;
    private float rotationInput;

    private bool isAttacking = false; // 攻撃状態を追跡

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isAttacking) // 攻撃中は他の動作をブロック
        {
            HandleMovement();
            HandleRotation();
            HandleJump();
        }

        HandleAttack(); // 攻撃処理は常に確認
    }

    private void HandleMovement()
    {
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        bool isMoving = false;

        // 移動処理
        if (Input.GetKey(KeyCode.W))
        {
            moveSpeed = isSprinting ? sprintSpeed : speed;
            isMoving = true;
            anim.SetBool("walking", true);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            moveSpeed = isSprinting ? -sprintSpeed : -speed;
            isMoving = true;
        }
        else if(Input.GetKeyUp(KeyCode.W))
        {
            anim.SetBool("walking", false);
        }
        else
        {
            moveSpeed = 0;
        }

        if (!inJumping)
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }

        // アニメーション設定
        anim.SetBool("walking", isMoving && !isSprinting);
        anim.SetBool("dassyu", isSprinting);
    }

    private void HandleRotation()
    {
        rotationInput = Input.GetKey(KeyCode.RightArrow) ? rotationSpeed * Time.deltaTime :
                        Input.GetKey(KeyCode.LeftArrow) ? -rotationSpeed * Time.deltaTime : 0;

        transform.Rotate(Vector3.up * rotationInput);
    }

    private void HandleJump()
    {
        if (onGround && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            onGround = false;
            inJumping = true;

            if (Input.GetKey(KeyCode.W))
            {
                rb.AddForce(transform.forward * forwardJumpForce, ForceMode.Impulse);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                rb.AddForce(-transform.forward * backwardJumpForce, ForceMode.Impulse);
            }

            anim.SetBool("walking", false);
        }
    }

    private void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            isAttacking = true; // 攻撃開始
            anim.SetTrigger("attack");

            // 攻撃終了を遅延で設定
            StartCoroutine(ResetAttack());
        }
    }

    private IEnumerator ResetAttack()
    {
        // 攻撃アニメーションの長さ（秒）に合わせて待機
        yield return new WaitForSeconds(1.0f); // アニメーションの長さに合わせる
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
