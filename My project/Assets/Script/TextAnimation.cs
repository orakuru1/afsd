using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextAnimation : MonoBehaviour
{
    [SerializeField]private Text text;
    [SerializeField]private string fullText;
    //[SerializeField]private float delay = 0.001f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetText(string str, Text T)
    {
        text = T;
        fullText = str;
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        text.text = ""; // 最初は空にする
        for (int i = 0; i <= fullText.Length; i++)
        {
            text.text = fullText.Substring(0, i); // 1文字ずつ追加
            yield return new WaitForSeconds(0.03f);
        }
    }
}
