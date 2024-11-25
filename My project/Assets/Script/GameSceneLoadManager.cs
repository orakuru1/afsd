using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class GameSceneLoadManager : MonoBehaviour
{
    public Transform player; // プレイヤーオブジェクト

    // セーブするデータ：シーンとプレイヤーの位置
    public void SaveGame()
    {
        // 現在のシーン名を保存
        string currentSceneName = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("SavedScene", currentSceneName);

        // プレイヤーの位置を保存
        PlayerPrefs.SetFloat("PlayerPosX", player.position.x);
        PlayerPrefs.SetFloat("PlayerPosY", player.position.y);
        PlayerPrefs.SetFloat("PlayerPosZ", player.position.z);

        PlayerPrefs.Save();
        Debug.Log("ゲームデータがセーブされました！");
    }

    // データをロードしてシーンを移動
    public void LoadGame()
    {
        // 保存されたシーン名を取得
        string savedSceneName = PlayerPrefs.GetString("SavedScene", "");

        if (!string.IsNullOrEmpty(savedSceneName))
        {
            // シーンを非同期でロードする
            SceneManager.LoadScene(savedSceneName);
            
            // シーンがロードされた後に位置を復元する
            StartCoroutine(LoadPlayerPosition());
        }
        else
        {
            Debug.LogWarning("保存されたシーンがありません！");
        }
    }

    // プレイヤーの位置を復元するコルーチン
    private IEnumerator LoadPlayerPosition()
    {
        // シーンがロードされるまで待機
        yield return new WaitForSeconds(0.5f); // 適宜調整する

        // 保存された位置データを取得
        float x = PlayerPrefs.GetFloat("PlayerPosX", player.position.x);
        float y = PlayerPrefs.GetFloat("PlayerPosY", player.position.y);
        float z = PlayerPrefs.GetFloat("PlayerPosZ", player.position.z);

        // プレイヤーの位置を更新
        player.position = new Vector3(x, y, z);
        Debug.Log("プレイヤーの位置がロードされました！");
    }
}
