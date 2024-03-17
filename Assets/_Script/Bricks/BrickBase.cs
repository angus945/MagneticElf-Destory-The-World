using System;
using System.Collections;
using System.Collections.Generic;
using AngusChanToolkit.Unity;
using UnityEngine;

public class GlobalEvent_BrickBreak : EventArgs_Global
{
    public BrickBase brick;

    public GlobalEvent_BrickBreak(BrickBase brick)
    {
        this.brick = brick;
    }
}
public class BrickBase : MonoBehaviour
{
    public int maxHealth;
    public int health;
    public Item dropItem;

    public LayerMask ballLayer;
    public float fireRadius;
    public float fireRate;
    public BrickBullet bulletPrefab;
    float fireTimer;

    void Update()
    {
        fireTimer += Time.deltaTime;

        Collider[] colliders = Physics.OverlapSphere(transform.position, fireRadius, ballLayer);
        if (colliders.Length > 0)
        {
            if (fireTimer > fireRate)
            {
                fireTimer = 0;

                Vector3 direction = (colliders[0].transform.position - transform.position).normalized;
                BrickBullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                direction.y = 0;
                bullet.direction = direction;
            }
        }
    }


    public void Damage(int damage)
    {
        health -= damage;
        GlobalOberserver.TriggerEvent(this, new GlobalEvent_HealthUpdated(transform, health, maxHealth));

        if (health <= 0)
        {
            GlobalOberserver.TriggerEvent<GlobalEvent_BrickBreak>(this, new GlobalEvent_BrickBreak(this));

            if (dropItem != null)
            {
                Instantiate(dropItem, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
}
