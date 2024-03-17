using System.Collections;
using System.Collections.Generic;
using AngusChanToolkit.Unity;
using UnityEngine;

public class PickupBall : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Ball>(out Ball ball))
        {
            GlobalOberserver.TriggerEvent(this, new GlobalEvent_AddBall(ball.ballName));
            Destroy(collision.gameObject);
        }
    }

}
