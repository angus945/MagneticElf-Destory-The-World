using System;
using System.Collections;
using System.Collections.Generic;
using AngusChanToolkit.Unity;
using UnityEngine;

public class ArenaBound : MonoBehaviour
{
    [SerializeField] float speed;

    Vector3 target;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        target = transform.position;

        GlobalOberserver.AddListener<GlobalEvent_ArenaPush>(Event_OnArenaPush);
    }
    void Update()
    {
        if (transform.position.z < target.z)
        {
            rb.MovePosition(transform.position + Vector3.forward * Time.deltaTime * speed);
        }
    }
    void Event_OnArenaPush(object sender, EventArgs e)
    {
        target.z += 1;
    }
}
