using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickBullet : MonoBehaviour
{

    public float speed = 10;
    public int damage = 1;

    public Vector3 direction;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        transform.up = direction;
    }
    void FixedUpdate()
    {
        rb.MovePosition(transform.position + direction * speed * Time.fixedDeltaTime);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Ball ball))
        {
            ball.Damage(damage);
        }

        Destroy(gameObject);
    }
}
