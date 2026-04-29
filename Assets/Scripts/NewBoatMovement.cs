using UnityEngine;

public class NewBoatMovement : MonoBehaviour
{
    [Header("ตั้งค่าการเคลื่อนที่")]
    public float moveSpeed = 15f;      // แรงขับเคลื่อนไปข้างหน้า
    public float rotationSpeed = 200f; // ความไวในการหมุนพวงมาลัย

    private Rigidbody2D rb;
    private float moveInput;
    private float rotationInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // ป้องกันบั๊กลืมตั้งค่าใน Inspector
        rb.gravityScale = 0f;
    }

    void Update()
    {
        // รับค่าปุ่มกดใน Update เพื่อไม่ให้พลาดจังหวะการกดของผู้เล่น
        // W, S หรือ ลูกศรขึ้นลง
        moveInput = Input.GetAxis("Vertical");

        // A, D หรือ ลูกศรซ้ายขวา
        rotationInput = Input.GetAxis("Horizontal");
    }

    void FixedUpdate()
    {
        // 1. ระบบขับเคลื่อน (เดินหน้า-ถอยหลัง)
        if (Mathf.Abs(moveInput) > 0.05f)
        {
            // ดันเรือไปในทิศทางที่หัวเรือกำลังหันไป (transform.up)
            rb.AddForce(transform.up * moveInput * moveSpeed);
        }

        // 2. ระบบพวงมาลัย (เลี้ยวซ้าย-ขวา)
        if (Mathf.Abs(rotationInput) > 0.05f)
        {
            // ใส่เครื่องหมายลบ (-) เพื่อให้กด D (ขวา) แล้วหมุนตามเข็มนาฬิกา
            rb.AddTorque(-rotationInput * rotationSpeed * Time.fixedDeltaTime);
        }
    }
}