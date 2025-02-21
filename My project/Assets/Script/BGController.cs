using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGController : MonoBehaviour
{
    public GameObject BG; 
    public Player player;

    private Vector3 worldPosition = new Vector3();

    void Start()
    {
        if (BG != null && player == null)
        {
            BG.transform.localScale = new Vector3(0.075f,0.07f,0f);
        }
    }
    
    void LateUpdate()
    {
        if (BG != null && player != null)
        {
            worldPosition = new Vector3(-0.21f,2.1f,0f) + transform.position;
        }
        else
        {
            worldPosition = new Vector3(-0.2f,2.4f,0f) + transform.position;
        }

        BG.transform.position = worldPosition;

    }


    private void OnDestroy()
    {
        // キャラクターが削除された際にHPバーも削除
        if (BG != null)
        {
            Destroy(BG);
        }
    }
}
