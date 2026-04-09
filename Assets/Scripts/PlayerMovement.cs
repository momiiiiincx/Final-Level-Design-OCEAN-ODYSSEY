using UnityEngine;
using UnityEngine.InputSystem; // ต้องเพิ่ม Namespace นี้

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    void Awake()
    {
        // Unity 6 แนะนำให้ดึง Component ใน Awake
        rb = GetComponent<Rigidbody2D>();

        // ตั้งค่า Rigidbody อัตโนมัติป้องกันการลืม
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    // ฟังก์ชันนี้จะถูกเรียกโดย Component "Player Input" (Message: OnMove)
    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void FixedUpdate()
    {
        // การคำนวณความเร็ว (Velocity) ใน Unity 6 สำหรับ Top-down
        // วิธีนี้ให้ความรู้สึกที่ลื่นไหลและแม่นยำกว่า MovePosition
        rb.linearVelocity = moveInput * moveSpeed;
    }
}