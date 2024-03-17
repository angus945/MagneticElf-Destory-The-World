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
    public Item dropItem;

    public int health;

    public void Damage(int damage)
    {
        health -= damage;
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
