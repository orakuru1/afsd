using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform target;       // 追従するキャラクター（例えばプレイヤー）のTransform
    public Vector3 offset;         // カメラのオフセット（キャラクターからの位置）
    public float smoothSpeed = 0.125f; // カメラ追従のスムーズさ
    public float rotationSpeed = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void LateUpdate()
    {
    if (Input.GetMouseButton(1)) // 右クリックで回転を有効にする場合
    {
        float horizontal = Input.GetAxis("Mouse X") * rotationSpeed;
        offset = Quaternion.AngleAxis(horizontal, Vector3.up) * offset;
    }

        // 目的地の位置を計算
        Vector3 desiredPosition = target.position + offset;
        // スムーズにカメラを移動させる
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // カメラをキャラクターの方向に向ける
        transform.LookAt(target);
    }
}
