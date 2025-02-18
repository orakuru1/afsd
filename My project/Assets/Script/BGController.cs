using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGController : MonoBehaviour
{
    public GameObject BG; 
    public Player player;

    void Start()
    {
            Vector3 scale = BG.transform.localScale; 
            scale.x = 0.6f; 
            BG.transform.localScale = scale; 

    }
    
    void Update()
    {
        
        if (BG != null && player != null)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position + new Vector3(-0.1f,1f,0) * 2.1f);

            if (screenPosition.z > 0) 
            {
                BG.transform.position = screenPosition;
                BG.SetActive(true);
            }
            else
            {
                BG.SetActive(false);
            }
        }
        else
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position + new Vector3(-0.2f,1f,0) * 2.4f);

            if (screenPosition.z > 0) 
            {
                BG.transform.position = screenPosition;
                BG.SetActive(true);
            }
            else
            {
                BG.SetActive(false);
            }
        }
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
