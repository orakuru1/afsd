using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSelection : MonoBehaviour
{
    [SerializeField] private GameObject skillButtonPrefab; // 技ボタンのプレハブ
    [SerializeField] private GameObject skillpanel;
    [SerializeField] public Transform skillListParent; // ボタンを配置する親オブジェクト

    private List<GameObject> insta = new List<GameObject>();

    void Start()
    {
        GenerateSkillButtons();
    }

    void GenerateSkillButtons()
    {
        //GameObject button = Instantiate(skillpanel)
        foreach (Skill skill in BattleManager.players[0].skills)
        {
            GameObject button = Instantiate(skillButtonPrefab, skillListParent);
            insta.Add(button);
            Debug.Log(button);
            button.GetComponentInChildren<Text>().text = skill.skillName;

        }
    }

}
