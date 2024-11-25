using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSaveLoadManager : MonoBehaviour
{

    public void SaveCurrentScene()
    {
        //現在のシーンの名前を取得
        string currentSceneName = SceneManager.GetActiveScene().name;


        //playerprefsを使ってシーン名を保存
        PlayerPrefs.SetString("SavedScene", currentSceneName);
        PlayerPrefs.Save();

        Debug.Log("シーン" + currentSceneName + "がセーブされました");
    }

    public void LoadSavedScene()
    {
        //保存されたシーン名を取得
        string savedSceneName = PlayerPrefs.GetString("SavedScene", "");

        if(!string.IsNullOrEmpty(savedSceneName))
        {
            //シーンをロードする
            SceneManager.LoadScene(savedSceneName);
            Debug.Log("シーン" + savedSceneName + "がロードされました");
        }
        else
        {
            Debug.LogWarning("保存されたシーンがありません");
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
