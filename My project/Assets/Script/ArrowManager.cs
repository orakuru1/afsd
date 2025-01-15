using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowManager : MonoBehaviour
{
    public RectTransform arrow; //矢印のUI
    public Canvas canvas; //矢印の親canvas
    private Transform currentTarget; // 現在のターゲット（敵）
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // currentTargetが壊れた参照か確認
        if (currentTarget == null || !currentTarget.gameObject.activeInHierarchy)
        {
            UpdateTarget(); // ターゲットを更新
        }

        if (currentTarget != null)
        {
            // ターゲットのワールド座標をスクリーン座標に変換
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(currentTarget.position + Vector3.up * 4.0f);

            // 矢印をターゲットの上に配置
            arrow.position = screenPosition;
        }
    }

    public void SetTarget(Transform  target)
    {
        // ターゲットを設定
        currentTarget = target;
    }

    public void UpdateTarget()
    {
        // 敵リストから一番左の敵を取得
        Enemy leftMostEnemy = EnemyManager.GetLeftMostEnemy();

        if (leftMostEnemy != null)
        {
            SetTarget(leftMostEnemy.transform);
        }
        else
        {
            SetTarget(null); // 敵がいない場合は矢印を非表示
        }
    }
}
