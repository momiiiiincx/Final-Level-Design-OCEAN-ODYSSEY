using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Dialogue Content")]
    public string[] messagesDefault;
    public string[] messagesPhaseInProgress;
    public string[] messagesPhaseComplete;
    public string[] messagesNextPhase;

    [HideInInspector] public string[] messages;

    [Header("UI References")]
    public TextMeshProUGUI dialogueUI;
    public GameObject visualPanel;

    [Header("Settings")]
    public float fadeDuration = 0.5f;
    public UnityEvent onDialogueComplete;

    private Image panelImage;
    private bool isPlayerInside = false;
    private bool isDialogueActive = false;
    private int currentMessageIndex = 0;

    private void Start()
    {
        if (visualPanel != null)
        {
            panelImage = visualPanel.GetComponent<Image>();
            visualPanel.SetActive(false);
        }
        SetAlpha(0);
        dialogueUI.text = "";
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.tKey.wasPressedThisFrame && isPlayerInside)
        {
            if (!isDialogueActive) StartDialogue();
            else NextMessage();
        }
    }

    private void StartDialogue()
    {
        QuestManager qm = Object.FindFirstObjectByType<QuestManager>();
        if (qm != null)
        {
            if (qm.questPhase == 0) messages = messagesDefault;
            else if (qm.questPhase == 1) messages = (qm.woodCount >= 1 && qm.ropeCount >= 1) ? messagesPhaseComplete : messagesPhaseInProgress;
            else if (qm.questPhase == 2) messages = (qm.foundMechanic) ? messagesPhaseComplete : messagesPhaseInProgress;
            else messages = messagesNextPhase;
        }
        isDialogueActive = true;
        currentMessageIndex = 0;
        ShowDialogue();
    }

    private void NextMessage()
    {
        currentMessageIndex++;
        if (currentMessageIndex < messages.Length) dialogueUI.text = messages[currentMessageIndex];
        else CompleteDialogue();
    }

    private void CompleteDialogue()
    {
        HideDialogue();
        isDialogueActive = false;

        QuestManager qm = Object.FindFirstObjectByType<QuestManager>();
        if (qm != null)
        {
            // --- ส่วนของพระราชา ---
            // ถ้ายังไม่ได้เริ่มเควสเลย (Phase 0) ให้รัน StartPhase1
            if (qm.questPhase == 0)
            {
                qm.StartPhase1();
                Debug.Log("เริ่มเควสเก็บของ (Phase 1)");
            }
            // ถ้าอยู่ Phase 1 และเก็บของครบแล้ว ให้รัน StartPhase2 (ไปหาช่าง)
            else if (qm.questPhase == 1 && qm.woodCount >= 1 && qm.ropeCount >= 1)
            {
                qm.StartPhase2();
                Debug.Log("ผ่านเควสเก็บของ -> ไปหาช่างเรือ (Phase 2)");
            }

            // --- ส่วนของช่างเรือ ---
            // ถ้าคุยกับช่างเรือในขณะที่อยู่ Phase 2 (ตามหาคน) และเจอตัวช่างแล้ว
            else if (qm.questPhase == 2 && qm.foundMechanic)
            {
                qm.StartPhase3(); // สั่งเปลี่ยนเป็น Phase 3 (ไปที่เรือ)
                Debug.Log("เปลี่ยนเป็น Phase 3: ไปที่เรือได้เลย!");
            }

            // รัน Event อื่นๆ ที่ตั้งไว้ใน Inspector (ถ้ามี)
            onDialogueComplete.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) { if (other.CompareTag("Player")) isPlayerInside = true; }
    private void OnTriggerExit2D(Collider2D other) { if (other.CompareTag("Player")) { isPlayerInside = false; HideDialogue(); isDialogueActive = false; } }

    void ShowDialogue()
    {
        if (messages.Length > 0 && dialogueUI != null)
        {
            dialogueUI.text = messages[0];
            if (visualPanel != null) visualPanel.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(FadeProcess(1f));
        }
    }

    void HideDialogue() { StopAllCoroutines(); StartCoroutine(FadeProcess(0f, true)); }

    IEnumerator FadeProcess(float targetAlpha, bool disableAfter = false)
    {
        float currentTime = 0;
        Color pColor = (panelImage != null) ? panelImage.color : Color.clear;
        Color tColor = dialogueUI.color;
        float startAlpha = tColor.a;
        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, currentTime / fadeDuration);
            if (panelImage != null) panelImage.color = new Color(pColor.r, pColor.g, pColor.b, newAlpha);
            dialogueUI.color = new Color(tColor.r, tColor.g, tColor.b, newAlpha);
            yield return null;
        }
        if (disableAfter && targetAlpha == 0) { if (visualPanel != null) visualPanel.SetActive(false); dialogueUI.text = ""; }
    }

    void SetAlpha(float alpha)
    {
        if (panelImage != null) panelImage.color = new Color(panelImage.color.r, panelImage.color.g, panelImage.color.b, alpha);
        if (dialogueUI != null) dialogueUI.color = new Color(dialogueUI.color.r, dialogueUI.color.g, dialogueUI.color.b, alpha);
    }
}