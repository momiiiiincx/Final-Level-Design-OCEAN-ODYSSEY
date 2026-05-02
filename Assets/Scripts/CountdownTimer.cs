using UnityEngine;
using TMPro; // สำคัญมาก: ต้องมีบรรทัดนี้ถึงจะสั่งงาน TextMeshPro ได้
using UnityEngine.SceneManagement; 

public class CountdownTimer : MonoBehaviour
{
    [Header("ตั้งค่าเวลา")]
    public float timeRemaining = 60f; // ตั้งเวลาเป็นวินาที (60 = 1 นาที)
    public bool timerIsRunning = false;

    [Header("UI ที่ต้องการแสดงผล")]
    public TMP_Text timerText; // ช่องสำหรับลาก UI ตัวหนังสือมาใส่

    private void Start()
    {
        // ให้เวลาเริ่มเดินทันทีที่เริ่มเกม
        timerIsRunning = true;
    }

    private void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                // Time.deltaTime คือเวลาที่ผ่านไปในแต่ละเฟรม จะทำให้เวลาลดลงตามจริงเป๊ะๆ
                timeRemaining -= Time.deltaTime; 
                UpdateTimerDisplay(timeRemaining);
            }
            else
            {
                // เมื่อเวลาหมด (น้อยกว่าหรือเท่ากับ 0)
                Debug.Log("หมดเวลา!");
                timeRemaining = 0;
                timerIsRunning = false;
                UpdateTimerDisplay(timeRemaining); // อัปเดตให้แสดงเป็น 00:00

                // ทำคำสั่ง Game Over หรือโหลดด่านใหม่ตรงนี้ได้เลย
                // SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
            }
        }
    }

    // ฟังก์ชันสำหรับจัดรูปแบบตัวเลขให้ออกมาเป็น นาที:วินาที
    private void UpdateTimerDisplay(float timeToDisplay)
    {
        // คำนวณหา นาที และ วินาที
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);  
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        // นำไปแสดงที่ UI โดยใช้รูปแบบ 00:00
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        
        // หมายเหตุ: ถ้าอยากให้โชว์แค่วินาทีอย่างเดียวโดยไม่มีนาที ให้ใช้บรรทัดล่างนี้แทนครับ
        // timerText.text = Mathf.CeilToInt(timeToDisplay).ToString();
    }
}