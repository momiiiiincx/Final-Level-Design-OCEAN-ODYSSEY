using UnityEngine;

public class GameManagerGhost : MonoBehaviour
{
    // สร้างเป็น Singleton เพื่อให้สคริปต์อื่นเรียกใช้ได้ง่ายๆ โดยไม่ต้อง Find()
    public static GameManagerGhost Instance;

    [Header("ตั้งค่าเวลา (วินาที)")]
    public float timeToWin = 240f; // 4 นาที = 240 วินาที

    private float currentTime = 0f;
    private bool isGameOver = false;

    void Awake()
    {
        // จัดการ Singleton
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    void Start()
    {
        // ทำให้เวลาเดินตามปกติ (เผื่อเล่นใหม่แล้วเวลายังหยุดอยู่)
        Time.timeScale = 1f; 
        isGameOver = false;
    }

    void Update()
    {
        if (isGameOver) return;

        // นับเวลาเพิ่มขึ้นเรื่อยๆ
        currentTime += Time.deltaTime;

        // เช็คว่ารอดครบกำหนดหรือยัง
        if (currentTime >= timeToWin)
        {
            GameWin();
        }
    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        Debug.Log("💀 โดนผีจับได้! GAME OVER 💀");
        
        // หยุดเวลาในเกม (ผีและผู้เล่นจะหยุดนิ่ง)
        Time.timeScale = 0f; 

        // TODO: ตรงนี้คุณสามารถเพิ่มโค้ดเรียกหน้าต่าง UI Game Over ขึ้นมาแสดงได้
    }

    public void GameWin()
    {
        if (isGameOver) return;
        isGameOver = true;

        Debug.Log("🎉 รอดชีวิตครบ 4 นาที! YOU WIN 🎉");
        
        // หยุดเวลาในเกม
        Time.timeScale = 0f;

        // TODO: ตรงนี้คุณสามารถเพิ่มโค้ดเรียกหน้าต่าง UI ชนะ ขึ้นมาแสดงได้
    }
}