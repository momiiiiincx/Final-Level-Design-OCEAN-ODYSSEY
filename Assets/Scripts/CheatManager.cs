using UnityEngine;
using UnityEngine.InputSystem; // สำหรับ Unity 6

public class CheatManager : MonoBehaviour
{
    [Header("Cheat Settings")]
    public Transform playerTransform;    // ลากตัวละคร Player มาใส่
    public Transform boatPosition;      // ลากจุดวาร์ปหน้าเรือมาใส่ (สร้าง Empty Object ไว้)

    void Update()
    {
        // ตรวจสอบการกดปุ่ม F1
        if (Keyboard.current != null && Keyboard.current.f1Key.wasPressedThisFrame)
        {
            ApplyCheat();
        }
    }

    void ApplyCheat()
    {
        // 1. ค้นหา QuestManager
        QuestManager qm = Object.FindFirstObjectByType<QuestManager>();

        if (qm != null)
        {
            // 2. ตั้งค่าระบบทั้งหมดให้เสร็จสิ้น (ข้ามไป Phase 3)
            qm.isQuestActive = true;
            qm.questPhase = 3;
            qm.woodCount = 3;
            qm.ropeCount = 3;
            qm.foundMechanic = true;

            Debug.Log("<color=yellow>[Cheat]</color> Quest Phase 3 Unlocked!");

            // 3. วาร์ปตัวละครไปยังตำแหน่งหน้าเรือ
            if (playerTransform != null && boatPosition != null)
            {
                playerTransform.position = boatPosition.position;
                Debug.Log("<color=green>[Cheat]</color> Player teleported to Boat!");
            }
            else
            {
                Debug.LogWarning("ลืมลาก Player หรือ BoatPosition ใส่ใน CheatManager นะครับ!");
            }
        }
        else
        {
            Debug.LogError("หา QuestManager ไม่เจอ!");
        }
    }
}