using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example : MonoBehaviour
{
    public GameObject prf; //インスペクタで設定するプレハブ
    public GameObject prf1;
    public int numberOfCharacters = 50; // 配置するキャラクターの数
    public int numberOFCharact = 50;
    public Vector3 spawnAreaMin = new Vector3(50, -5, -900);
    public Vector3 spawnMax = new Vector3(900, -5, 900);

    // Start is called before the first frame update
    void Start()
    {
        //指定された数だけキャラクターをランダムな位置に配置
        for(int i = 0; i < numberOfCharacters; i++ )
        {
            //ランダムな位置を生成
            float x = Random.Range(spawnAreaMin.x, spawnMax.x);
            float y = Random.Range(spawnAreaMin.y, spawnMax.y);
            float z = Random.Range(spawnAreaMin.z, spawnMax.z);
            Vector3 randomPosition = new Vector3(x, y, z);

            //プレハブをシーンにインスタンス化
            Instantiate(prf, randomPosition, Quaternion.identity);
        }

        for(int i = 0; i < numberOFCharact; i++)
        {
            //ランダムな位置を生成
            float x = Random.Range(spawnAreaMin.x, spawnMax.x);
            float y = Random.Range(spawnAreaMin.y, spawnMax.y);
            float z = Random.Range(spawnAreaMin.z, spawnMax.z);
            Vector3 randomPosition = new Vector3(x, y, z);

            //プレハブをシーンにインスタンス化
            Instantiate(prf1, randomPosition, Quaternion.identity);
        }

        //spawnRotationをQuaaternionに変更
        //Quaternion rotation = Quaternion.Euler(spawnRosition1);
        //プレハブをシーンにインスタンス化
        //Instantiate(prf, randomPosition, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
