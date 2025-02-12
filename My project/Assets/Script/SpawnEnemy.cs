using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [SerializeField]private GameObject go;
    void Start()
    {
        GameObject iti = Instantiate(go,new Vector3(2f,1f,1f),Quaternion.identity);
        GameObject ni = Instantiate(go,new Vector3(1f,1f,1f),Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
