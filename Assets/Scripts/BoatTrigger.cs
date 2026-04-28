using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

public class BoatTrigger : MonoBehaviour
{
    [Header("UI References")]
    public GameObject interactionPrompt;
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    [Header("Settings")]
    public float typingSpeed = 0.05f;

    private bool isPlayerAtBoat = false;
    private bool isMessageShown = false;

    void Start()
    {
        if (interactionPrompt != null) interactionPrompt.SetActive(false);
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerAtBoat = true;
            QuestManager qm = Object.FindFirstObjectByType<QuestManager>();
            if (qm != null && qm.questPhase == 3 && !isMessageShown)
                if (interactionPrompt != null) interactionPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerAtBoat = false;
            if (interactionPrompt != null) interactionPrompt.SetActive(false);
        }
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        if (isPlayerAtBoat && Keyboard.current.eKey.wasPressedThisFrame)
        {
            Debug.Log("กด E ที่เรือแล้ว!"); // เช็คว่าปุ่มทำงานไหม

            QuestManager qm = Object.FindFirstObjectByType<QuestManager>();
            if (qm == null)
            {
                Debug.LogError("หา QuestManager ไม่เจอในฉาก!");
                return;
            }

            Debug.Log("ปัจจุบันอยู่ใน Phase: " + qm.questPhase); // เช็คว่า Phase ตรงไหม

            if (qm.questPhase == 3 && !isMessageShown)
            {
                if (dialoguePanel != null && dialogueText != null)
                {
                    ShowDialogue();
                    qm.questFinished = true;
                }
                else
                {
                    Debug.LogError("ลืมลาก DialoguePanel หรือ Text ใส่ในสคริปต์ที่ตัวเรือครับ!");
                }
            }
        }
    }

    void ShowDialogue()
    {
        isMessageShown = true;
        if (interactionPrompt != null) interactionPrompt.SetActive(false);
        if (dialoguePanel != null) dialoguePanel.SetActive(true);
        StartCoroutine(TypeFinalMessage("This war negotiation will face many obstacles. I wish Leon a safe journey."));
    }

    IEnumerator TypeFinalMessage(string line)
    {
        dialogueText.text = "";
        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}