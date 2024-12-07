using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int health = 200;

    public float maxHealth = 200;
    public float currentHealth;

    private HealthBarManager healthBarManager;

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health < 0) health = 0;
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }

        Debug.Log("プレイヤーの体力: " + health);
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (healthBarManager != null)
        {
            healthBarManager.UpdateHealth(currentHealth, maxHealth);
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} が倒されました！");
        //Destroy(gameObject);
    }

    void Start()
    {
        currentHealth = maxHealth;
        healthBarManager = GetComponent<HealthBarManager>();
        UpdateHealthBar();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
