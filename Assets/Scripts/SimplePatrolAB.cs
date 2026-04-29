using UnityEngine;
using System.Collections;

public class SimplePatrolAB : MonoBehaviour
{
    [Header("Movement Points")]
    public Transform pointA;
    public Transform pointB;

    [Header("Settings")]
    public float speed = 3f;
    public float waitTime = 1f;

    private Transform target;
    private bool isWaiting = false;

    void Start()
    {
        // เริ่มต้นที่จุด A
        if (pointA != null)
        {
            target = pointA;
            transform.position = pointA.position;
        }
    }

    void Update()
    {
        // ถ้าไม่มีเป้าหมาย หรือกำลังรออยู่ ไม่ต้องทำอะไร
        if (target == null || isWaiting) return;

        // เคลื่อนที่เข้าหาเป้าหมาย (Vector2 สำหรับงาน 2D)
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // เช็คว่าถึงจุดหมายหรือยัง
        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            StartCoroutine(WaitAndSwitch());
        }
    }

    IEnumerator WaitAndSwitch()
    {
        isWaiting = true;

        // หยุดรอตามเวลาที่กำหนด
        yield return new WaitForSeconds(waitTime);

        // สลับเป้าหมาย A -> B หรือ B -> A
        target = (target == pointA) ? pointB : pointA;

        isWaiting = false;
    }

    // วาดเส้นให้เห็นในหน้า Scene (ช่วยให้จัดจุดง่ายขึ้น)
    void OnDrawGizmos()
    {
        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(pointA.position, pointB.position);
            Gizmos.DrawSphere(pointA.position, 0.2f);
            Gizmos.DrawSphere(pointB.position, 0.2f);
        }
    }
}