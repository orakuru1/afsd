using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSpawn : MonoBehaviour
{
    public string Name = "Charactar"; // 名前
    public int Health = 100; // 体力
    public Transform SpawnPoint; // 生成する位置
    private GameObject Instance;

    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Spawn()
    {
        GameObject prefab = (GameObject)Resources.Load (Name);
        Instance = Instantiate(prefab, SpawnPoint.position, Quaternion.identity);
    }

}
