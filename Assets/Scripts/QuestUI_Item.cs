using UnityEngine;
using TMPro;

public class QuestUI_Item : MonoBehaviour
{
    public string itemTag;                // 👈 ต้องมี
    public int requiredAmount;            // 👈 ต้องมี
    public TextMeshProUGUI text;          // 👈 ต้องมี

    void Update()
    {
        int have = PlayerInventory.instance.Get(itemTag);

        if (have >= requiredAmount)
        {
            text.text = $"{have}/{requiredAmount} ✔";
            text.color = Color.green;
        }
        else
        {
            text.text = $"{have}/{requiredAmount}";
            text.color = Color.white;
        }
    }
}