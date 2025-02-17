using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveButton : MonoBehaviour
{

    Button button;

    // Start is called before the first frame update
    void Start()
    {
        //button.onClick.AddListener(OnClickReceiveButton);
    }

    public void OnClickReceiveButton()
    {
        GameObject nowPlayer;
        nowPlayer = GameObject.Find("Player");
        nowPlayer.GetComponent<Player>().SaveRespawnPosition();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
