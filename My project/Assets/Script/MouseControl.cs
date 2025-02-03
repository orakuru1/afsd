using UnityEngine;
using UnityEngine.EventSystems;

public class MouseControl : MonoBehaviour
{
    public float mouseSensitivity = 100f;  // マウス感度
    public Transform playerBody;           // プレイヤーのボディ

    private float xRotation = 0f;          // 縦の回転角度

    void Start()
    {
        // 初期状態でカーソルを非表示にする（ゲームが始まったらカーソルがロックされ、非表示）
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // UI上にカーソルがある場合（ボタンなどにカーソルが乗っている）
        if (EventSystem.current.IsPointerOverGameObject())
        {
            // UIの上にカーソルがあればカーソルを表示
            Cursor.visible = true;
        }
        else
        {
            // UI外ではカーソルを非表示
            Cursor.visible = false;
        }

        // マウスの移動量を取得
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // 縦の回転角度を更新し、カメラの回転に反映
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -10f, 45f);  // 上下の視点移動を制限

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        
        // 横の回転角度をキャラクターに適用
        playerBody.Rotate(Vector3.up * mouseX);
    }
}

