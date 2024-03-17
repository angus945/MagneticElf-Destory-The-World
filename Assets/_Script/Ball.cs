using System;
using System.Collections;
using System.Collections.Generic;
using AngusChanToolkit.Unity;
using Unity.VisualScripting;
using UnityEngine;

public class GlobalEvent_PickUpItem : EventArgs_Global
{

}
public class Ball : MonoBehaviour
{
    public string ballName;

    [SerializeField] int maxHealth;
    [SerializeField] int health;

    public Vector3 direction;
    [SerializeField] float speed;

    public LayerMask itemLayer;
    public float pickupRadius;

    Rigidbody rigidbody;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    void Update()
    {
        float distance = Vector3.Distance(transform.position, Paddle.instance.transform.position);
        if (distance > 30)
        {
            GlobalOberserver.TriggerEvent(this, new GlobalEvent_AddBall(ballName));
            Destroy(gameObject);
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, pickupRadius, itemLayer);
        if (colliders.Length > 0)
        {
            if (colliders[0].TryGetComponent<Item>(out Item item))
            {
                GlobalOberserver.TriggerEvent(this, new GlobalEvent_PickUpItem());
                Destroy(colliders[0].gameObject);
            }
        }
    }
    void FixedUpdate()
    {
        Vector3 velocity = direction * speed;
        rigidbody.MovePosition(velocity * Time.fixedDeltaTime + transform.position);
    }

    void OnCollisionEnter(Collision collision)
    {
        direction = Vector3.Reflect(direction, collision.contacts[0].normal);

        if (collision.gameObject.TryGetComponent<BrickBase>(out BrickBase brick))
        {
            brick.Damage(1);
        }
    }

    public void Damage(int damage)
    {
        health -= damage;
        GlobalOberserver.TriggerEvent(this, new GlobalEvent_HealthUpdated(transform, health, maxHealth));

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
