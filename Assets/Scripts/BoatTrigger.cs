using UnityEngine;
using UnityEngine.InputSystem; // จำเป็นสำหรับ Unity 6 / New Input System

public class BoatTrigger : MonoBehaviour
{
<<<<<<< Updated upstream
    [Header("UI References")]
    public GameObject interactionPrompt;
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    [Header("Fade Settings")]
    public Image fadeImage; // เปลี่ยนจาก CanvasGroup เป็น Image
    public float fadeDuration = 1.5f;
    public string nextSceneName = "Level2";

    [Header("Settings")]
    public float typingSpeed = 0.05f;

=======
>>>>>>> Stashed changes
    private bool isPlayerAtBoat = false;

    // เช็กว่าผู้เล่นเดินเข้ามาที่เรือหรือยัง (2D)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerAtBoat = true;
            Debug.Log("<color=cyan>[Boat]</color> อยู่ที่เรือแล้ว (กด E เพื่อออกเรือ)");
        }
    }

    // เช็กว่าผู้เล่นเดินออกจากเรือ
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerAtBoat = false;
        }
    }

    private void Update()
    {
        // เงื่อนไข: ต้องอยู่ใกล้เรือ + กด E + ต้องอยู่ใน Phase 3 (คุยกับช่างซ่อมเรือเสร็จแล้ว)
        if (isPlayerAtBoat && Keyboard.current.eKey.wasPressedThisFrame)
        {
            QuestManager qm = Object.FindFirstObjectByType<QuestManager>();

            if (qm != null)
            {
                if (qm.questPhase == 3)
                {
                    qm.questFinished = true;
                    Debug.Log("<color=yellow>★★ QUEST COMPLETE! ★★</color>");
                    // คุณสามารถใส่โค้ดเปลี่ยนฉาก เช่น SceneManager.LoadScene("Ending"); ตรงนี้ได้เลย
                }
                else
                {
                    Debug.Log("<color=red>[Boat]</color> เรือยังซ่อมไม่เสร็จ หรือเจ้ายังไม่มีภารกิจนี้!");
                }
            }
        }
    }
}