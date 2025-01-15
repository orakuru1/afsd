using UnityEngine;

public class ResetPosition : MonoBehaviour
{
    // オブジェクトの初期位置を記録する変数
    private Vector3 startPosition;

    void Start()
    {
        // 初期位置を記録
        startPosition = transform.position;
    }

    void Update()
    {
        // 現在の座標を確認
        if (transform.position.y < -30)
        {
            // 初期位置に戻す
            transform.position = startPosition;
            
        }
    }
}

