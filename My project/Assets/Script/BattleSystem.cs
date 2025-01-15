using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    [SerializeField] private GameObject attackbotton; // 技選択UIパネル
    [SerializeField] private GameObject escapebotton; // 技選択UIパネル

    // Start is called before the first frame update
    void Start()
    {
    }

    public void OnAttackButtonPressed()
    {
        // 技選択UIを表示
        attackbotton.SetActive(!attackbotton.activeSelf);
        escapebotton.SetActive(!escapebotton.activeSelf);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
