using System;
using System.Collections.Generic;
using AngusChanToolkit.Unity;
using UnityEngine;

internal class GlobalEvent_HealthUpdated : EventArgs_Global
{
    public Transform target;
    public int health;
    public int maxHealth;

    public GlobalEvent_HealthUpdated(Transform target, int health, int maxHealth)
    {
        this.target = target;
        this.health = health;
        this.maxHealth = maxHealth;
    }
}
public class UIHealthBars : MonoBehaviour
{

    [SerializeField] int barCount;
    [SerializeField] UIHealthBar barPrefab;

    Queue<UIHealthBar> pool = new Queue<UIHealthBar>();
    Dictionary<Transform, UIHealthBar> activeHealthBar = new Dictionary<Transform, UIHealthBar>();

    void Start()
    {
        Camera camera = Camera.main;

        for (int i = 0; i < barCount; i++)
        {
            UIHealthBar bar = Instantiate(barPrefab, transform);
            bar.gameObject.SetActive(false);
            pool.Enqueue(bar);
        }

        GlobalOberserver.AddListener<GlobalEvent_HealthUpdated>(ActiveHealthBar);
    }
    void ActiveHealthBar(object sender, EventArgs e)
    {
        GlobalEvent_HealthUpdated args = e as GlobalEvent_HealthUpdated;

        if (activeHealthBar.TryGetValue(args.target, out UIHealthBar bar))
        {
            bar.SetTarget(args.target, args.health / (float)args.maxHealth);

            if (args.health <= 0)
            {
                activeHealthBar.Remove(args.target);
                pool.Enqueue(bar);
                bar.gameObject.SetActive(false);
            }
        }
        else
        {
            if (args.health <= 0)
            {
                // activeHealthBar.Remove(args.target);
                // bar.gameObject.SetActive(false);
                // pool.Enqueue(bar);
            }
            else
            {
                bar = pool.Dequeue();
                bar.SetTarget(args.target, args.health / (float)args.maxHealth);
                bar.gameObject.SetActive(true);

                activeHealthBar.Add(args.target, bar);
            }

        }


    }
}
