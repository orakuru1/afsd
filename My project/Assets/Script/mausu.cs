using UnityEngine;

public class mausu : MonoBehaviour
{
    public float mouseSensitivity = 100f;  // マウス感度
    public Transform playerBody;           // プレイヤーのボディ

    private float xRotation = 0f;          // 縦の回転角度

    void Start()
    {
        // カーソルをロックして非表示にする
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // マウスの移動量を取得
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // 縦の回転角度を更新し、カメラの回転に反映
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);  // 上下の視点移動を制限

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        
        // 横の回転角度をキャラクターに適用
        playerBody.Rotate(Vector3.up * mouseX);
    }
}

