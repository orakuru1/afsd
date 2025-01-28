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
        LockCursor();
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow; // フルスクリーンウィンドウモードを有効にする
    }

    void Update()
    {
        if ((canvas != null && canvas.activeSelf) || (imageObject != null && imageObject.activeSelf))
        {
            if (isCursorLocked)
                UnlockCursor();
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!isCursorLocked)
                LockCursor();
        }

        if (isCursorLocked)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

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
