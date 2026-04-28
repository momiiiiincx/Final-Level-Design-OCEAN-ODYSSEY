using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BoatController : MonoBehaviour
{
    [Header("ตั้งค่าความเร็วเรือ")]
    public float normalSpeed = 5f;    
    public float boostSpeed = 8f;     
    public float boostDuration = 2f;  
    
    private float currentSpeed;
    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = normalSpeed; 
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        // ขับเรือตามปกติ ชนทรายหรือหินก็จะติดเองตามฟิสิกส์
        Vector2 newPos = rb.position + movement.normalized * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPos);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Whirlpool"))
        {
            StartCoroutine(SpeedBoostRoutine());
        }
    }

    IEnumerator SpeedBoostRoutine()
    {
        currentSpeed = boostSpeed;
        yield return new WaitForSeconds(boostDuration);
        currentSpeed = normalSpeed;
    }
}