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
    private string[] messages;

    [Header("UI References")]
    public TextMeshProUGUI dialogueUI;
    public GameObject visualPanel;
    public GameObject interactionPrompt; // <-- เพิ่มช่องใส่รูปปุ่ม "T" ตรงนี้

    [Header("Settings")]
    public float typingSpeed = 0.04f;
    public float fadeDuration = 0.5f;
    public UnityEvent onDialogueComplete;

    private Image panelImage;
    private bool isPlayerInside = false;
    private bool isDialogueActive = false;
    private int currentMessageIndex = 0;
    private Coroutine typingCoroutine;

    void Start()
    {
        if (visualPanel != null)
        {
            panelImage = visualPanel.GetComponent<Image>();
            visualPanel.SetActive(false);
        }

        // เริ่มเกมให้ปิดปุ่มบอกใบ้ไว้ก่อน
        if (interactionPrompt != null) interactionPrompt.SetActive(false);

        SetAlpha(0);
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.tKey.wasPressedThisFrame && isPlayerInside)
        {
            if (!isDialogueActive) StartDialogue();
            else NextMessage();
        }
    }

    void StartDialogue()
    {
        QuestManager qm = Object.FindFirstObjectByType<QuestManager>();
        if (qm == null) return;

        if (qm.questPhase == 0) messages = messagesDefault;
        else if (qm.questPhase == 1) messages = (qm.woodCount >= 3 && qm.ropeCount >= 3) ? messagesPhaseComplete : messagesPhaseInProgress;
        else if (qm.questPhase == 2) messages = (qm.foundMechanic) ? messagesPhaseComplete : messagesPhaseInProgress;
        else messages = messagesNextPhase;

        if (messages == null || messages.Length == 0) return;

        // เมื่อเริ่มคุย ให้ปิดปุ่มบอกใบ้ (Interaction Prompt)
        if (interactionPrompt != null) interactionPrompt.SetActive(false);

        isDialogueActive = true;
        currentMessageIndex = 0;
        ShowDialogue();
    }

    void NextMessage()
    {
        currentMessageIndex++;
        if (currentMessageIndex < messages.Length)
        {
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            typingCoroutine = StartCoroutine(TypeText(messages[currentMessageIndex]));
        }
        else CompleteDialogue();
    }

    void CompleteDialogue()
    {
        HideDialogue();
        isDialogueActive = false;

        // เมื่อคุยจบ ถ้าผู้เล่นยังอยู่ใน Collider ให้เปิดปุ่มบอกใบ้คืนมา
        if (isPlayerInside && interactionPrompt != null) interactionPrompt.SetActive(true);

        QuestManager qm = Object.FindFirstObjectByType<QuestManager>();
        if (qm != null)
        {
            if (qm.questPhase == 0) qm.StartPhase1();
            else if (qm.questPhase == 1 && qm.woodCount >= 3 && qm.ropeCount >= 3) qm.StartPhase2();
            else if (qm.questPhase == 2 && qm.foundMechanic) qm.StartPhase3();
            onDialogueComplete.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            // เมื่อเดินเข้าใกล้ และยังไม่ได้คุย ให้โชว์ปุ่มบอกใบ้
            if (!isDialogueActive && interactionPrompt != null) interactionPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            // เมื่อเดินออก ให้ปิดทั้งปุ่มบอกใบ้และบทสนทนา
            if (interactionPrompt != null) interactionPrompt.SetActive(false);
            HideDialogue();
            isDialogueActive = false;
        }
    }

    void ShowDialogue()
    {
        visualPanel.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(FadeProcess(1f));
        typingCoroutine = StartCoroutine(TypeText(messages[0]));
    }

    IEnumerator TypeText(string line)
    {
        dialogueUI.text = "";
        foreach (char letter in line.ToCharArray())
        {
            dialogueUI.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    void HideDialogue() { StopAllCoroutines(); StartCoroutine(FadeProcess(0f, true)); }

    IEnumerator FadeProcess(float targetAlpha, bool disableAfter = false)
    {
        float currentTime = 0;
        float startAlpha = dialogueUI.color.a;
        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, currentTime / fadeDuration);
            SetAlpha(alpha);
            yield return null;
        }
        if (disableAfter && targetAlpha == 0) { visualPanel.SetActive(false); dialogueUI.text = ""; }
    }

    void SetAlpha(float alpha)
    {
        if (panelImage != null) panelImage.color = new Color(panelImage.color.r, panelImage.color.g, panelImage.color.b, alpha);
        if (dialogueUI != null) dialogueUI.color = new Color(dialogueUI.color.r, dialogueUI.color.g, dialogueUI.color.b, alpha);
    }
}