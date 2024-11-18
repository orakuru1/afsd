using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectile : MonoBehaviour
{
    public GameObject projectilePrefab; // 弾丸のPrefab
    public Transform firePoint; // 発射位置
    public float fireRate = 0.5f; // 発射速度
    private float nextFireTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }  
        
    }

    void Shoot()
    {
        // 弾丸を発射位置に生成し、向きを設定
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
    }

}
