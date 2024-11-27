using UnityEngine;

public class ScaleControl : MonoBehaviour
{
    // 縮小速度
    public float scaleSpeed = 1.0f;
    // 最小スケールの制限
    public float minScaleY = 0.5f;
    // 元のスケールを保存する変数
    private Vector3 originalScale;

    void Start()
    {
        // オブジェクトの初期スケールを保存
        originalScale = transform.localScale;
    }

    void Update()
    {
        // 左Controlキーが押されている間
        if (Input.GetKey(KeyCode.LeftControl))
        {
            // 現在のスケールを取得
            Vector3 currentScale = transform.localScale;

            // Y軸のスケールを縮小
            currentScale.y -= scaleSpeed * Time.deltaTime;

            // 最小スケールを下回らないように制限
            currentScale.y = Mathf.Max(currentScale.y, minScaleY);

            // スケールを適用
            transform.localScale = currentScale;
        }
        else
        {
            // Controlキーが離された場合、元のスケールに戻す
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * scaleSpeed);
        }
    }
}

