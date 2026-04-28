using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class OctopusAI : MonoBehaviour
{
    [Header("ตั้งค่าปลาหมึก")]
    public float chaseSpeed = 4f;     
    private bool isChasing = true;    

    private Transform boatTransform;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject boatObj = GameObject.FindGameObjectWithTag("Player");
        if (boatObj != null) boatTransform = boatObj.transform;
    }

    void FixedUpdate()
    {
        if (!isChasing || boatTransform == null) return;

        // ว่ายตามเรือไปเรื่อยๆ (ทะลุหินได้ถ้าตั้งเป็น Trigger)
        Vector2 newPos = Vector2.MoveTowards(rb.position, (Vector2)boatTransform.position, chaseSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
    }

    // ปลาหมึกจะหยุดเมื่อ "ตัวมันเอง" ไปชนกับสิ่งที่มี Tag ว่า Sand
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Sand"))
        {
            isChasing = false;
            rb.velocity = Vector2.zero;
            Debug.Log("หมึกว่ายมาเกยตื้นเองแล้ว!");
        }
        else if (other.CompareTag("Player"))
        {
            Debug.Log("💀 โดนปลาหมึกจับได้!");
        }
    }
}