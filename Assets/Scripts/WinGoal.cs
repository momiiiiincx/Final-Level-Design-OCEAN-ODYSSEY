using UnityEngine;
using UnityEngine.SceneManagement; // ต้องใช้ตัวนี้สำหรับการเปลี่ยนซีน

public class WinGoal : MonoBehaviour
{
    // ตั้งชื่อซีนที่คุณต้องการให้โหลดเมื่อชนะ เช่น "WinScene"
    public string nextSceneName = "WinScene"; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ตรวจสอบว่าสิ่งที่เข้ามาชนมี Tag ว่า "Player" (เรือของเรา) หรือไม่
        if (other.CompareTag("Player"))
        {
            Debug.Log("เข้าเส้นชัยแล้ว! ชนะด่าน!");
            
            // นำคอมเมนต์ออกเพื่อใช้งานคำสั่งโหลดซีนถัดไป หรือซีนแสดงความยินดี
            // SceneManager.LoadScene(nextSceneName); 
        }
    }
}