using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    // 回転速度
    public float rotationSpeed = 100f;

    void Update()
    {
        // 上下方向の回転 (W: 上, S: 下)
        if (Input.GetKey(KeyCode.W))
        {
            transform.Rotate(Vector3.left * rotationSpeed * Time.deltaTime); // 上を向く
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime); // 下を向く
        }

        // 左右方向の回転 (A: 左, D: 右)
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.up * -rotationSpeed * Time.deltaTime); // 左を向く
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime); // 右を向く
        }
    }
}

