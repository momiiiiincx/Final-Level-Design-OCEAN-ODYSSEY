using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public void RetryLevel()
    {
        // ดึงชื่อด่านล่าสุดที่จำไว้ (ถ้าหาไม่เจอ ให้ตั้งค่าเริ่มต้นกลับไปหน้า "Departure")
        string levelToLoad = PlayerPrefs.GetString("LastLevel", "Departure");
        
        // โหลดกลับไปด่านนั้น
        SceneManager.LoadScene(levelToLoad);
    }
}