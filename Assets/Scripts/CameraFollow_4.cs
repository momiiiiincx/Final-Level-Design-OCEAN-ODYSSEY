using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform target; // ลาก Player (ตัวแม่) มาใส่ที่นี่
    [SerializeField] private float smoothSpeed = 0.125f; // ความสมูท (0-1)
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10); // ระยะห่างจากตัวละคร

    void LateUpdate()
    {
        if (target == null) return;

        // ตำแหน่งที่กล้องควรจะไป
        Vector3 desiredPosition = target.position + offset;

        // ใช้ Lerp เพื่อให้กล้องขยับตามแบบนุ่มนวล
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // อัปเดตตำแหน่งกล้อง
        transform.position = smoothedPosition;
    }
}