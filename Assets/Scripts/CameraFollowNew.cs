using UnityEngine;

public class CameraFollowNew : MonoBehaviour
{
    [Header("เป้าหมายที่กล้องจะตาม")]
    public Transform target;

    [Header("ระยะห่างกล้อง (Z ต้องติดลบเสมอ)")]
    public Vector3 offset = new Vector3(0f, 0f, -10f);

    [Header("ความสมูท (ค่ายิ่งน้อย = ยิ่งตามติดเร็ว)")]
    [Range(0.01f, 1f)]
    public float smoothTime = 0.125f;

    // ตัวแปรที่ SmoothDamp ต้องใช้เก็บค่าความเร็วชั่วคราว
    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        // เช็คว่ามีเป้าหมายให้ตามไหม
        if (target != null)
        {
            // ตำแหน่งปลายทางที่กล้องควรไปอยู่
            Vector3 targetPosition = target.position + offset;

            // ใช้ SmoothDamp แทน Lerp ภาพจะเนียนและลดอาการสั่นกระตุกได้ดีกว่ามาก
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }
}
