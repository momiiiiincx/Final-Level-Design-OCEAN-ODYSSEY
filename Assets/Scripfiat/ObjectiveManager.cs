using UnityEngine;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    public TextMeshProUGUI objectiveText;

    void Start()
    {
        SetObjective("Press the statues in the correct order"); // 🔥 เริ่มเกม
    }

    public void SetObjective(string text)
    {
        objectiveText.text = "Objective: " + text;
    }
}
