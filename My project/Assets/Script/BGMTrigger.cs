using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMTrigger : MonoBehaviour
{
    public AudioSource bgmSource; //再生するBGM
    private bool hasPlayed = false; //1回だけ再生するフラグ

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !hasPlayed)
        {
            bgmSource.Play();
            hasPlayed = true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
