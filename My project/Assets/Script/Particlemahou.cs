using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particlemahou : MonoBehaviour
{
    private new ParticleSystem particleSystem;
    [SerializeField] private Player player;
    float time = 5;

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        Destroy(gameObject,time);
    }

    void OnParticleCollision(GameObject other)
    {
        EnemyHP enemy = other.GetComponent<EnemyHP>();
        if (enemy != null)
        {
            enemy.TakeDamage(player.attack);
        }

        particleSystem.Stop();
        Destroy(gameObject);
    }
}