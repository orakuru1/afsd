using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    public ArrowManager arrowManager; //矢印を制御するスクリプト
    void Start()
    {
        // ArrowManagerを取得
        arrowManager = FindObjectOfType<ArrowManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 左クリック
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // タグが"Enemy"のオブジェクトをクリックした場合
                if (hit.collider.CompareTag("Enemy"))
                {
                    Transform clickedEnemy = hit.collider.transform;
                    arrowManager.SetTarget(clickedEnemy);
                }
            }
        }
    }
}
