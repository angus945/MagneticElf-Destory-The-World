using System;
using System.Collections;
using System.Collections.Generic;
using AngusChanToolkit.Unity;
using UnityEngine;

public class GlobalEvent_AddBall : EventArgs_Global
{
    public string ball;

    public GlobalEvent_AddBall(string ball)
    {
        this.ball = ball;
    }
}
public class GlobalEvent_BallCountChange : EventArgs_Global
{
    public string ball;
    public int count;

    public GlobalEvent_BallCountChange(string ball, int count)
    {
        this.ball = ball;
        this.count = count;
    }
}

[System.Serializable]
public class Inventory
{
    public Ball ball;
    public int count
    {
        get { return _count; }
        set
        {
            _count = value; GlobalOberserver.TriggerEvent(this, new GlobalEvent_BallCountChange(ball.ballName, _count));
        }
    }

    public int _count;
}
public class Paddle : MonoBehaviour
{
    public static Paddle instance;

    [Header("Paddle Movement")]
    [SerializeField] float speed;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float pushDuration;

    [Header("Paddle Shooting")]
    [SerializeField] Inventory[] inventories;
    [SerializeField] Transform shootPoint;
    [SerializeField] float shootInterval;

    Rigidbody rb;

    float moveTargetX;
    float moveTargetZ;

    Coroutine shootCoroutine;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        GlobalOberserver.AddListener<GlobalEvent_ArenaPush>(Event_OnArenaPush);
        GlobalOberserver.AddListener<GlobalEvent_AddBall>(Event_OnAddBall);
    }
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100, groundLayer))
            {
                moveTargetX = hit.point.x;
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (shootCoroutine != null)
            {
                StopCoroutine(shootCoroutine);
            }
            shootCoroutine = StartCoroutine(ShootBalls());
        }
    }
    void FixedUpdate()
    {
        Vector3 target = transform.position;
        if (Mathf.Abs(transform.position.x - moveTargetX) > 0.1f)
        {
            float direction = Mathf.Sign(moveTargetX - transform.position.x);
            target += new Vector3(direction, 0, 0) * speed * Time.fixedDeltaTime;
        }

        if (transform.position.z < moveTargetZ)
        {
            target += Vector3.forward * speed * Time.fixedDeltaTime;
        }
        rb.MovePosition(target);
    }

    IEnumerator ShootBalls()
    {
        foreach (var inventory in inventories)
        {
            while (inventory.count > 0)
            {
                Ball ball = Instantiate(inventory.ball, transform.position + shootPoint.localPosition, Quaternion.identity);
                ball.direction = new Vector3(1, 0, 1).normalized;
                inventory.count--;

                yield return new WaitForSeconds(shootInterval);
            }
        }
    }

    void Event_OnArenaPush(object sender, EventArgs e)
    {
        moveTargetZ += 1;
    }
    private void Event_OnAddBall(object sender, EventArgs e)
    {
        GlobalEvent_AddBall addBall = e as GlobalEvent_AddBall;
        foreach (var inventory in inventories)
        {
            if (inventory.ball.ballName == addBall.ball)
            {
                inventory.count++;
            }
        }

    }

}
