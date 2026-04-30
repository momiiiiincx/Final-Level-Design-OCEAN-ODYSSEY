using UnityEngine;

public class Character : MonoBehaviour
{
    public float baseMovementSpeed = 5f;
    
    private float currentMovementSpeed;
    private SpriteRenderer playerSprite;
    
    // 1. เพิ่มตัวแปร Animator
    private Animator anim; 

    void Start()
    {
        currentMovementSpeed = baseMovementSpeed;
        playerSprite = GetComponent<SpriteRenderer>();
        
        // 2. ดึง Animator Component มาใช้งาน
        anim = GetComponent<Animator>(); 
    }

    void Update()
    {
        Move();
    }
    
    void Move()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        
        if (playerSprite != null)
        {
            if (moveX > 0) 
            {
                playerSprite.flipX = false; 
            }
            else if (moveX < 0) 
            {
                playerSprite.flipX = true;  
            }
        }

        Vector2 movement = new Vector2(moveX, moveY).normalized;
        transform.Translate(movement * currentMovementSpeed * Time.deltaTime);

        // 3. ส่วนนี้คือหัวใจสำคัญที่ทำให้ Animation ทำงาน!
        if (anim != null)
        {
            // ถ้ามีการกดปุ่มเดิน (moveX หรือ moveY ไม่เท่ากับ 0) isMoving จะเป็น true
            bool isMoving = (moveX != 0 || moveY != 0);
            
            // ส่งค่า true/false ไปให้ parameter ที่ชื่อ "isWalking" ใน Animator
            anim.SetBool("IsWalking", isMoving);
        }
    }
}