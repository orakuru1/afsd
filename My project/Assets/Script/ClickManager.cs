using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    public ArrowManager arrowManager; //矢印を制御するスクリプト
    [SerializeField]private AudioSource audioSource;
    [SerializeField]private AudioClip audioClip;
    private Camera maincamera;
    void Start()  
    {
        // ArrowManagerを取得
        arrowManager = FindObjectOfType<ArrowManager>();
        maincamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 左クリック
        {
            Ray ray = maincamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // タグが"Enemy"のオブジェクトをクリックした場合
                if (hit.collider.CompareTag("Enemy"))
                {
                    if(arrowManager.arrow.gameObject.activeSelf)
                    {
                        Debug.Log("dsafds");
                        audioSource.PlayOneShot(audioClip);
                    }

                    Transform clickedEnemy = hit.collider.transform;
                    arrowManager.SetTarget(clickedEnemy);
                }
            }
        }
    }
}
