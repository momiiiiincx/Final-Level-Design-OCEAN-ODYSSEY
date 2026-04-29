using UnityEngine;

public class BoatMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 10f;      // ความเร็วในการเดินหน้า
    public float rotationSpeed = 150f; // ความเร็วในการหมุนเรือ

    private Rigidbody2D rb;
    private float moveInput;
    private float rotationInput;

    void Start()
    {
        // ดึงคอมโพเนนต์ Rigidbody2D มาใช้งาน
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // รับค่าจากคีย์บอร์ด (W,S / Up,Down / A,D / Left,Right)
        moveInput = Input.GetAxis("Vertical");      // เดินหน้า-ถอยหลัง
        rotationInput = Input.GetAxis("Horizontal"); // เลี้ยวซ้าย-ขวา
    }

    void FixedUpdate()
    {
        // ใช้ FixedUpdate สำหรับงานที่เกี่ยวกับ Physics
        MoveBoat();
        RotateBoat();
    }

    void MoveBoat()
    {
        // ดันเรือไปตามทิศทางที่หัวเรือหันไป (transform.up)
        if (moveInput != 0)
        {
            rb.AddForce(transform.up * moveInput * moveSpeed);
        }
    }

    void RotateBoat()
    {
        // หมุนเรือตามแรงที่รับมา
        float rotation = rotationInput * rotationSpeed * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation - rotation);
    }
}
