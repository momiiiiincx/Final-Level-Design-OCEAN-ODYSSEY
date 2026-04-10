using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    [Header("Quest Data")]
    public int questPhase = 0; // 0: เริ่มต้น, 1: เก็บของ, 2: ตามหาคน
    public bool isQuestActive = false;

    [Header("Phase 1: Items")]
    public int woodCount = 0;
    public int ropeCount = 0;

    [Header("Phase 2: Person")]
    public bool foundTargetPerson = false;

    [Header("UI References")]
    public GameObject questPanel;
    public TextMeshProUGUI questStatusText;

    void Start() { if (questPanel != null) questPanel.SetActive(false); }

    void Update()
    {
        if (isQuestActive) UpdateQuestUI();
    }

    public void StartCollectQuest() // เรียกเมื่อคุยครั้งแรก
    {
        isQuestActive = true;
        questPhase = 1;
        if (questPanel != null) questPanel.SetActive(true);
    }

    public void StartFindPersonQuest() // เรียกเมื่อคุยครั้งที่สองหลังเก็บของครบ
    {
        questPhase = 2;
        Debug.Log("เริ่มเควสตามหาคน!");
    }

    void UpdateQuestUI()
    {
        if (questPhase == 1)
        {
            questStatusText.text = $"<b>GOAL: COLLECT</b>\nWood: {woodCount}/1\nRope: {ropeCount}/1";
            if (woodCount >= 1 && ropeCount >= 1)
                questStatusText.text = "<color=green>COMPLETE!</color>\nReturn to King";
        }
        else if (questPhase == 2)
        {
            questStatusText.text = foundTargetPerson ?
                "<color=green>FOUND PERSON!</color>\nReturn to King" :
                "<b>GOAL: FIND HELP</b>\nLook for someone in the forest";
        }
    }
}