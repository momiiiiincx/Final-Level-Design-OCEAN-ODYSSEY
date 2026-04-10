using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Dialogue Arrays")]
    public string[] messagesDefault;        // คำพูดก่อนรับเควส
    public string[] messagesDuringCollect;  // คำพูดตอนกำลังเก็บของ (ยังไม่ครบ)
    public string[] messagesPhase1Complete; // คำพูดตอนเก็บของครบ (และจะสั่งเควส 2)
    public string[] messagesDuringFind;     // คำพูดตอนกำลังตามหาคน (เควส 2)

    [HideInInspector] public string[] messages; // ตัวแปรหลักที่จะใช้แสดงผล

    [Header("UI References")]
    public TextMeshProUGUI dialogueUI;
    public GameObject visualPanel;

    [Header("Settings")]
    public float fadeDuration = 0.5f;
    public bool hasQuestAfterTalk = false;
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
        if (Keyboard.current != null && Keyboard.current.tKey.wasPressedThisFrame)
        {
            if (isPlayerInside)
            {
                if (!isDialogueActive) StartDialogue();
                else NextMessage();
            }
        }
    }

    private void StartDialogue()
    {
        QuestManager qm = FindObjectOfType<QuestManager>();

        // เลือกว่าจะพูดอะไรตามสถานะเควส
        if (qm != null)
        {
            if (qm.questPhase == 0)
            {
                messages = messagesDefault;
                hasQuestAfterTalk = true;
            }
            else if (qm.questPhase == 1)
            {
                if (qm.woodCount >= 1 && qm.ropeCount >= 1)
                {
                    messages = messagesPhase1Complete;
                    hasQuestAfterTalk = true; // เพื่อให้จบแล้ว Invoke ไปรัน Phase 2
                }
                else
                {
                    messages = messagesDuringCollect;
                    hasQuestAfterTalk = false;
                }
            }
            else if (qm.questPhase == 2)
            {
                messages = messagesDuringFind;
                hasQuestAfterTalk = false;
            }
        }

        isDialogueActive = true;
        currentMessageIndex = 0;
        ShowDialogue();
    }

    private void NextMessage()
    {
        currentMessageIndex++;
        if (currentMessageIndex < messages.Length)
            dialogueUI.text = messages[currentMessageIndex];
        else
            CompleteDialogue();
    }

    private void CompleteDialogue()
    {
        HideDialogue();
        isDialogueActive = false;
        if (hasQuestAfterTalk && onDialogueComplete != null)
            onDialogueComplete.Invoke();
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