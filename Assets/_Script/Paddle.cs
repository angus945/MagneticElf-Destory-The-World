using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    public Ball ball;
    public int count;
}
public class Paddle : MonoBehaviour
{
    [Header("Paddle Movement")]
    [SerializeField] float speed;
    [SerializeField] LayerMask groundLayer;

    [Header("Paddle Shooting")]
    [SerializeField] Inventory[] inventories;
    [SerializeField] Transform shootPoint;
    [SerializeField] float shootInterval;

    Rigidbody rb;

    Vector3 moveTarget;

    Coroutine shootCoroutine;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100, groundLayer))
            {
                moveTarget = new Vector3(hit.point.x, transform.position.y, transform.position.z);
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
        if (Vector3.Distance(transform.position, moveTarget) > 0.1f)
        {
            Vector3 direction = (moveTarget - transform.position).normalized;
            direction.y = 0;
            direction.z = 0;
            rb.MovePosition(transform.position + direction * speed * Time.fixedDeltaTime);
        }
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

}
