using System.Collections;
using System.Collections.Generic;
using AngusChanToolkit.Unity;
using UnityEngine;

public class PickupBall : MonoBehaviour
{
    public float lostProbability = 0.5f;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Ball>(out Ball ball))
        {
            if (Random.value > lostProbability)
            {
                GlobalOberserver.TriggerEvent(this, new GlobalEvent_AddBall(ball.ballName));
            }

            Destroy(collision.gameObject);
        }
    }

}
