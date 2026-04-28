using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AdvancedCustomPatrolAI : MonoBehaviour
{
    // สร้าง Class สำหรับเก็บข้อมูลแต่ละจุด
    [System.Serializable]
    public class WaypointData
    {
        public string pointName;      // ชื่อจุด (ตั้งไว้ให้ไม่งง)
        public Transform pointTransform;
        public string animationToPlay; // ชื่อท่าที่จะให้เล่นเมื่อถึงจุดนี้
        public float waitTime = 2f;    // เวลารอเฉพาะจุดนี้
    }

    [Header("Patrol Settings")]
    public List<WaypointData> waypoints = new List<WaypointData>();
    public string runAnimation = "Pawn_Run Axe_Yellow"; // ท่าเดินหลัก
    public float moveSpeed = 3f;

    private int currentPointIndex = 0;
    private bool isWaiting = false;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        if (waypoints.Count > 0)
        {
            transform.position = waypoints[0].pointTransform.position;
        }
    }

    void Update()
    {
        if (isWaiting || waypoints.Count == 0) return;

        // 1. เดินไปที่จุดหมาย
        WaypointData targetData = waypoints[currentPointIndex];

        if (targetData.pointTransform == null) return;

        anim.Play(runAnimation); // เล่นท่าเดิน
        transform.position = Vector2.MoveTowards(transform.position, targetData.pointTransform.position, moveSpeed * Time.deltaTime);

        // 2. หันหน้า
        FlipCharacter(targetData.pointTransform.position.x);

        // 3. เมื่อถึงจุดหมาย
        if (Vector2.Distance(transform.position, targetData.pointTransform.position) < 0.1f)
        {
            StartCoroutine(PerformPointAction(targetData));
        }
    }

    IEnumerator PerformPointAction(WaypointData data)
    {
        isWaiting = true;

        // เล่นท่าทางที่กำหนดไว้เฉพาะจุดนี้ (เช่น จุด A ให้ขุด, จุด B ให้ยืนพัก)
        if (!string.IsNullOrEmpty(data.animationToPlay))
        {
            anim.Play(data.animationToPlay);
        }

        // รอตามเวลาที่กำหนดไว้ในจุดนั้นๆ
        yield return new WaitForSeconds(data.waitTime);

        // ไปจุดถัดไป
        currentPointIndex = (currentPointIndex + 1) % waypoints.Count;

        isWaiting = false;
    }

    void FlipCharacter(float targetX)
    {
        if (targetX > transform.position.x)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
        }
        else if (targetX < transform.position.x)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
        }
    }

    void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Count < 2) return;
        Gizmos.color = Color.green;
        for (int i = 0; i < waypoints.Count; i++)
        {
            if (waypoints[i].pointTransform == null) continue;
            Gizmos.DrawSphere(waypoints[i].pointTransform.position, 0.2f);
            int next = (i + 1) % waypoints.Count;
            if (waypoints[next].pointTransform != null)
            {
                Gizmos.DrawLine(waypoints[i].pointTransform.position, waypoints[next].pointTransform.position);
            }
        }
    }
}