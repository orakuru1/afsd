using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGController : MonoBehaviour
{
    public GameObject BG; 
    public Player player;

    private Vector3 worldPosition = new Vector3();

    private Collider Characollider;

    private float objectHeight;

    void Start()
    {
        if (BG != null && player == null)
        {
            BG.transform.localScale = new Vector3(0.075f,0.07f,0f);
        }

        Characollider = GetComponent<Collider>();
        objectHeight = Characollider.bounds.size.y;
    }
    
    void LateUpdate()
    {
        if (BG != null && player != null)
        {
            worldPosition = new Vector3(-0.21f, objectHeight + 0.2f,0f) + transform.position;
        }
        else
        {
            worldPosition = new Vector3(-0.2f, objectHeight + 0.2f,0f) + transform.position;
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
