using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // ต้องมีตัวนี้เพื่อใช้ Image

public class BoatTrigger : MonoBehaviour
{
    [Header("UI References")]
    public GameObject interactionPrompt;
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    [Header("Fade Settings")]
    public Image fadeImage; // เปลี่ยนจาก CanvasGroup เป็น Image
    public float fadeDuration = 1.5f;
    public string nextSceneName = "NextSceneName";

    [Header("Settings")]
    public float typingSpeed = 0.05f;

    private bool isPlayerAtBoat = false;
    private bool isMessageShown = false;

    void Start()
    {
        if (interactionPrompt != null) interactionPrompt.SetActive(false);
        if (dialoguePanel != null) dialoguePanel.SetActive(false);

        // เริ่มต้นให้ Image สีดำโปร่งแสง (Alpha = 0)
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0;
            fadeImage.color = c;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerAtBoat = true;
            QuestManager qm = Object.FindAnyObjectByType<QuestManager>();
            if (qm != null && qm.questPhase == 3 && !isMessageShown)
            {
                if (interactionPrompt != null) interactionPrompt.SetActive(true);
            }
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

        if (isPlayerAtBoat && Keyboard.current.eKey.wasPressedThisFrame && !isMessageShown)
        {
            QuestManager qm = Object.FindFirstObjectByType<QuestManager>();
            if (qm != null && qm.questPhase == 3)
            {
                if (dialoguePanel != null && dialogueText != null)
                {
                    ShowDialogue();
                    qm.questFinished = true;
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

        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(FadeAndChangeScene());
    }

    IEnumerator FadeAndChangeScene()
    {
        if (fadeImage == null)
        {
            Debug.LogError("ลืมลาก Image สีดำมาใส่ในช่อง Fade Image นะครับ!");
            yield break;
        }

        float elapsedTime = 0;
        Color tempColor = fadeImage.color;

        // --- ส่วนการ Lerp Fade Out (ค่อยๆ ดำ) ---
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            // ปรับเฉพาะค่า Alpha (a) จาก 0 ไป 1
            tempColor.a = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            fadeImage.color = tempColor;
            yield return null;
        }

        tempColor.a = 1;
        fadeImage.color = tempColor;

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(nextSceneName);
    }
}