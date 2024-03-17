using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using AngusChanToolkit.Unity;
using System;

public class PlayerResources : MonoBehaviour
{
    [SerializeField] TMP_Text ballCountText;
    [SerializeField] TMP_Text itemCountText;

    [SerializeField] Ball ballPrefab;

    int itemCount;

    void Start()
    {
        GlobalOberserver.AddListener<GlobalEvent_PickUpItem>(Event_OnPickUpItem);
        GlobalOberserver.AddListener<GlobalEvent_BallCountChange>(Event_OnBallCountChange);
    }

    void Event_OnBallCountChange(object sender, EventArgs e)
    {
        GlobalEvent_BallCountChange e2 = (GlobalEvent_BallCountChange)e;
        ballCountText.text = e2.count.ToString();
    }

    void Event_OnPickUpItem(object sender, EventArgs e)
    {
        itemCount++;

        if (itemCount > 5)
        {
            GlobalOberserver.TriggerEvent(this, new GlobalEvent_AddBall(ballPrefab.ballName));
            itemCount -= 5;
        }

        itemCountText.text = itemCount.ToString();
    }
}
