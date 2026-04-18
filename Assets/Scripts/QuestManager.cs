using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    [Header("Quest Data")]
    public int questPhase = 0; // 0: เริ่ม, 1: เก็บของ, 2: หาช่าง, 3: ไปที่เรือ
    public bool isQuestActive = false;

    [Header("Phase 1: Items")]
    public int woodCount = 0;
    public int ropeCount = 0;

    [Header("Progress Status")]
    public bool foundMechanic = false;
    public bool questFinished = false;

    [Header("UI References")]
    public GameObject questPanel;
    public TextMeshProUGUI questStatusText;

    void Start()
    {
        if (questPanel != null) questPanel.SetActive(false);
    }

    void Update()
    {
        if (isQuestActive) UpdateQuestUI();
    }

    public void StartPhase1() { isQuestActive = true; questPhase = 1; questPanel.SetActive(true); }
    public void StartPhase2() { questPhase = 2; }
    public void StartPhase3() { questPhase = 3; }

    void UpdateQuestUI()
    {
        switch (questPhase)
        {
            case 1:
                questStatusText.text = $"<b>GOAL: COLLECT</b>\nWood: {woodCount}/1\nRope: {ropeCount}/1";
                if (woodCount >= 1 && ropeCount >= 1) questStatusText.text = "<color=green>COMPLETE!</color>\nReturn to King";
                break;
            case 2:
                questStatusText.text = foundMechanic ? "<color=green>MECHANIC FOUND!</color>\nTalk to him" : "<b>GOAL: REPAIR</b>\nGo to Boat mechanic";
                break;
            case 3:
                questStatusText.text = questFinished ? "<color=yellow>QUEST COMPLETE!</color>\nReady to sail!" : "<b>GOAL: FINAL</b>\nGo to Boat and Press E";
                break;
        }
    }
}