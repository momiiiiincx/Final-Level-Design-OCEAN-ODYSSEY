using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [Header("UI Text")]
    public TextMeshProUGUI woodText;
    public TextMeshProUGUI ropeText;

    void Update()
    {
        if (Inventory.instance == null) return;

        woodText.text = Inventory.instance.woodCount + " / 2";
        ropeText.text = Inventory.instance.ropeCount + " / 1";

        woodText.color = Inventory.instance.woodCount >= 2 ? Color.green : Color.white;
        ropeText.color = Inventory.instance.ropeCount >= 1 ? Color.green : Color.white;
    }
}
