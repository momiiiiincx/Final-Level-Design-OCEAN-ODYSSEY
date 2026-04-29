using UnityEngine;
using UnityEngine.SceneManagement; // 1. ต้องเพิ่มบรรทัดนี้เพื่อจัดการ Scene

public class GameManagerGhost : MonoBehaviour
{
    public static GameManagerGhost Instance;

    [Header("ตั้งค่าเวลา (วินาที)")]
    public float timeToWin = 240f; 

    [Header("การเปลี่ยน Scene")]
    public string winSceneName = "WinScene"; // 2. ชื่อ Scene ที่ต้องการให้โหลดเมื่อชนะ
    public string retrySceneName = "RetryScene";

    private float currentTime = 0f;
    private bool isGameOver = false;

    void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    void Start()
    {
        Time.timeScale = 1f; 
        isGameOver = false;
    }

    void Update()
    {
        if (isGameOver) return;

        currentTime += Time.deltaTime;

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
        
        // เวลาแพ้ ไม่ต้องหยุดเวลา Time.timeScale = 0f; แล้ว เพราะเราจะเปลี่ยน Scene เลย
        // 2. สั่งเปลี่ยนไปหน้า Retry ทันทีที่โดนจับ
        SceneManager.LoadScene(retrySceneName); 
    }

    public void GameWin()
    {
        if (isGameOver) return;
        isGameOver = true;

        Debug.Log("🎉 รอดชีวิตครบ 4 นาที! YOU WIN 🎉");
        
        // 3. เปลี่ยน Scene ไปยังหน้าที่ต้องการ
        // หมายเหตุ: ถ้าคุณต้องการให้คนเล่นเห็น UI ชนะก่อน แล้วค่อยกดปุ่มเปลี่ยน Scene 
        // ให้ย้ายบรรทัดข้างล่างนี้ไปไว้ที่ปุ่มกดแทนครับ
        SceneManager.LoadScene(winSceneName);
    }
}