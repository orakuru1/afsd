using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCharact : MonoBehaviour
{
    Rigidbody rb;
    Animator anim;
    Transform _transform;
    float _horizontal;
    float _vertical;
    bool MovuFlagForward = false;
    bool MovuFlagRight = false;
    bool MovuFlagLeft = false;
    bool MovuFlagBack = false;

    Quaternion _playerRotation;
    Vector3 _aim;
    // Start is called before the first frame update
    void Start()
    {
        // フレームレートを400psに設定する
        Application.targetFrameRate = 400;
        rb = GetComponent<Rigidbody>(); //rigidbodyを取得
        anim = GetComponent<Animator>();
        _transform = GetComponent<Transform>();
        _playerRotation = _transform.rotation;
    }

    void FixedUpdate()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");

        var _horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);

        _aim = _horizontalRotation * new Vector3(_horizontal, 0, _vertical).normalized;

        if (_aim.magnitude > 0.5f)
        {
            _playerRotation = Quaternion.LookRotation(_aim, Vector3.up);
        }

        _transform.rotation = Quaternion.RotateTowards(_transform.rotation, _playerRotation, 600 * Time.deltaTime); // Quaternion.RotateTowards(回転前の向き, 回転後の向き, 回転する速さ);


        //wキー(前方移動)
        if (MovuFlagForward)
        {
            Vector3 normal = new Vector3(0f, 0f, 7f);
            rb.AddForce(normal);
            MovuFlagForward = false;
        }
        // Sキー（後方移動）
        if (MovuFlagBack)
        {
            Vector3 normal = new Vector3(0f, 0f, -7f);
            rb.AddForce(normal);
            MovuFlagBack = false;
        }
        // Dキー（右移動）
        if (MovuFlagRight)
        {
            Vector3 normal = new Vector3(7f, 0f, 0f);
            rb.AddForce(normal);
            MovuFlagRight = false;
        }
        // Aキー（左移動）
        if (MovuFlagLeft)
        {
            Vector3 normal = new Vector3(-7f, 0f, 0f);
            rb.AddForce(normal);
            MovuFlagLeft = false;
        }


    }

    // Update is called once per frame
    void Update()
    {
        //wキー(前方移動)
        if (Input.GetKey(KeyCode.W))
        {
            MovuFlagForward = true;

        }

        // Sキー（後方移動）
        if (Input.GetKey(KeyCode.S))
        {
            MovuFlagBack = true;
        }

        // Dキー（右移動）
        if (Input.GetKey(KeyCode.D))
        {
            MovuFlagRight = true;
        }

        // Aキー（左移動）
        if (Input.GetKey(KeyCode.A))
        {
            MovuFlagLeft = true;
        }

        if (MovuFlagBack || MovuFlagForward || MovuFlagLeft || MovuFlagRight)//移動キーを押したら
        {
            anim.SetBool("walking", true);//歩くアニメーション
        }
        else
        {
            anim.SetBool("walking", false);//何も押されなかったら止まる
        }
    }
}