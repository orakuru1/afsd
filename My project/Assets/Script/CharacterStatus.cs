using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatus : MonoBehaviour
{
    public int maxHP = 100;
    public int currentHP = 100;
    public string CharacterName;

    public delegate void OnHPChanged(int newHP);
    public event OnHPChanged HPChanged;

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        HPChanged?.Invoke(currentHP);
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
