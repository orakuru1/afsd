using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveInterval = 2f; // 一定時間ごとの移動間隔
    public float moveDistance = 5f; // 一度の移動距離
    public float moveSpeed = 2f; // 移動速度

    private Vector3 targetPosition; // 次の目的地
    private float lastMoveTime; // 最後に移動した時間
    private bool isMoving = false;

    void Start()
    {
        // 初期化: 現在の位置を目的地に設定
        targetPosition = transform.position;
        lastMoveTime = Time.time;
    }

    void Update()
    {
        // 一定時間ごとに移動開始
        if (Time.time >= lastMoveTime + moveInterval && !isMoving)
        {
            SetNewTargetPosition();
            lastMoveTime = Time.time;
            isMoving = true;
        }

        // 移動中であれば目的地に向かう
        if (isMoving)
        {
            MoveToTarget();
        }
    }

    void SetNewTargetPosition()
    {
        // 現在位置からランダムな方向に次の目的地を決定
        Vector3 randomDirection = new Vector3(
            Random.Range(-1f, 1f), // X軸方向
            0,                    // Y軸は固定（地面に沿って移動）
            Random.Range(-1f, 1f) // Z軸方向
        ).normalized;

        targetPosition = transform.position + randomDirection * moveDistance;
    }

    void MoveToTarget()
    {
        // 目的地に向かって移動
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // 目的地に到達したら移動を終了
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            isMoving = false;
        }
    }
}

