using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManeger : MonoBehaviour
{
    public void StartNewGame()
    {
        //セーブデータを削除して最初から開始
        PlayerPrefs.DeleteKey("RespawnX");
        PlayerPrefs.DeleteKey("RespawnY");
        PlayerPrefs.DeleteKey("RespawnZ");
        PlayerPrefs.Save();
        SceneManager.LoadScene("GAMEMAPP"); //画面遷移
    }

    public void ContinueGame()
    {
        //セーブデータがあるばあのみ続きから開始
        if(PlayerPrefs.HasKey("RespawnX") && PlayerPrefs.HasKey("RespawnY") && PlayerPrefs.HasKey("RespawnZ"))
        {
            SceneManager.LoadScene("GAMEMAPP");
        }
        else
        {
            Debug.Log("セーブデータねーぞ！");
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
