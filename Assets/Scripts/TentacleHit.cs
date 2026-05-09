using UnityEngine;
using UnityEngine.SceneManagement; // สำคัญ: สำหรับโหลดซีน

public class TentacleHit : MonoBehaviour
{
    public string retrySceneName = "RetryScene"; // ชื่อ Scene ตอนแพ้

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ตรวจสอบว่าสิ่งที่ชนคือ Player หรือไม่
        if (other.CompareTag("Player"))
        {
            Debug.Log("💀 โดนหนวดจากใต้น้ำพุ่งชน! เด้งไปหน้า Game Over!");

            // สั่งเปลี่ยนไปหน้าแพ้ (Retry) ทันที
            SceneManager.LoadScene(retrySceneName);
        }
    }
}