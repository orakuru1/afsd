using UnityEngine;

public class ObjectMove : MonoBehaviour
{
    // 移動速度
    public float moveSpeed = 5f;

    void Update()
    {
        // 左右の入力を取得
        float horizontalInput = Input.GetAxis("Horizontal"); // "Horizontal" は A/D または ←/→ キーに対応

        // オブジェクトを左右に移動
        transform.Translate(Vector3.right * horizontalInput * moveSpeed * Time.deltaTime);
    }
}

