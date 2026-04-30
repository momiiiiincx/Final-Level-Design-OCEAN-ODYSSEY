using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // รับ Input จาก WASD หรือ Arrow Keys
        movement.x = Input.GetAxisRaw("Horizontal"); // A/D หรือ ←/→
        movement.y = Input.GetAxisRaw("Vertical");   // W/S หรือ ↑/↓

        // Normalize เพื่อป้องกันการเดินแนวทแยงเร็วกว่าปกติ
        movement = movement.normalized;
    }

    void FixedUpdate()
    {
        // เคลื่อนที่ผ่าน Rigidbody2D (แนะนำมากกว่า Transform)
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}