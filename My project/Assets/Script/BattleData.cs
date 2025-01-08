using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleData : MonoBehaviour
{
    public static BattleData Instance { get; private set; }

    //public GameObject enemyPrefab; // 戦闘シーンに持っていく敵のPrefab
    public string enemyName;       // 敵の名前などの情報
    public int enemyHealth;        // 敵の体力

    [SerializeField]Player player;

    private void Awake()
    {
        // シングルトンパターンを実装
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーン遷移時に削除されないようにする
        }
        else
        {
            Destroy(gameObject); // すでに存在する場合は削除
        }
    }
    public void SetEnemyData(string name, int health)
    {
        enemyName = name;
        enemyHealth = health;
    }

    void Start()
    {
        //player.skills.Add(new Skill { skillName = "Fire", damage = 30, description = "A ball of fire that burns enemies." });
        //player.equips.Add(new equipment{equipName = "woodsord",number = 5,description = "aaaaa"});
        //player.equips.Add(new equipment{equipName = "Ianpreat",number = 100,description = "iiiiii"});
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
