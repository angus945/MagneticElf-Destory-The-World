using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Vector3 direction;
    [SerializeField] float speed;

    Rigidbody rigidbody;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
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
}
