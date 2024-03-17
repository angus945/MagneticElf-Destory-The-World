using System.Collections;
using System.Collections.Generic;
using AngusChanToolkit.Unity;
using Unity.VisualScripting;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public string ballName;

    public Vector3 direction;
    [SerializeField] float speed;

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
