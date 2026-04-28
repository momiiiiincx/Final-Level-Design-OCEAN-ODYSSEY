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
    
    // เพิ่มตัวแปรอ้างอิงถึง SpriteRenderer
    private SpriteRenderer boatSprite;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // ดึง Component SpriteRenderer ที่อยู่บนตัวเรือมาใช้งาน
        boatSprite = GetComponent<SpriteRenderer>();
        
        currentSpeed = normalSpeed; 
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // --- ส่วนจัดการการหันหน้าของสไปรต์ ---
        if (boatSprite != null)
        {
            // ถ้าค่าแกน x มากกว่า 0 (กดเดินขวา) ให้แสดงภาพปกติ
            if (movement.x > 0) 
            {
                boatSprite.flipX = false; 
            }
            // ถ้าค่าแกน x น้อยกว่า 0 (กดเดินซ้าย) ให้กลับด้านภาพ
            else if (movement.x < 0) 
            {
                boatSprite.flipX = true;  
            }
        }
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