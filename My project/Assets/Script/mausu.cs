using UnityEngine;

public class mausu : MonoBehaviour
{
    public float mouseSensitivity = 100f;  // マウス感度
    public Transform playerBody;           // プレイヤーのボディ
    public GameObject canvas;              // 対象のキャンバスオブジェクト
    public GameObject imageObject;         // Imageを含むオブジェクト

    private float xRotation = 0f;          // 縦の回転角度
    private bool isCursorLocked = true;    // カーソルがロックされているかどうか

    void Start()
    {
        LockCursor(); // ゲーム開始時にカーソルをロック
    }

    void Update()
    {
        // 1. キャンバスまたは画像が表示されている場合、カーソルを表示
        if ((canvas != null && canvas.activeSelf) || (imageObject != null && imageObject.activeSelf))
        {
            if (isCursorLocked) // 既にロック解除されている場合は処理をしない
                UnlockCursor();
            return; // キャンバスや画像が表示されている間は、それ以降の処理をしない
        }

        // 2. 左クリックでカーソルを非表示にする
        if (Input.GetMouseButtonDown(0))
        {
            if (!isCursorLocked) // 既にロックされている場合は処理をしない
                LockCursor();
        }

        // 3. カメラ操作
        if (isCursorLocked)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);  // 上下の視点移動を制限

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }

    void LockCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        isCursorLocked = true;
    }

    void UnlockCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        isCursorLocked = false;
    }
}
