using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MPBar : MonoBehaviour
{
    public Slider MPSlider;

    public Text MpText;

    public Player player;

    public void UpdateMPBar()
    {
        if(MPSlider != null)
        {
            MPSlider.value = player.Mp;
            MpText.text = player.Mp.ToString();
        }

    }

    void Start()
    {
        if(MPSlider != null)
        {
            MPSlider.maxValue = player.MaxMp;
            MPSlider.value = player.Mp;
            MpText.text = player.Mp.ToString();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
