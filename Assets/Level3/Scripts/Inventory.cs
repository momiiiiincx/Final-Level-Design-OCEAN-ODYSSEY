using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public int woodCount = 0;
    public int ropeCount = 0;

    void Awake()
    {
        instance = this;
    }

    public void AddItem(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Wood:
                woodCount++;
                Debug.Log("ไม้: " + woodCount);
                break;
            case ItemType.Rope:
                ropeCount++;
                Debug.Log("เชือก: " + ropeCount);
                break;
        }
    }
}
