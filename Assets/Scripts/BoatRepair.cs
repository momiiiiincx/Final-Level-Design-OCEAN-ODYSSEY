using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class QuestItem
{
    public string tag;
    public int amount;
}

public class BoatRepair : MonoBehaviour
{
    public QuestItem[] requiredItems;

    private bool isCompleted = false; // 🔥 กันส่งซ้ำ
    public string nextSceneName; // 👉 ใส่ชื่อ Scene ใน Inspector

    public void TryRepair()
    {
        // ❌ ถ้าทำเสร็จแล้ว ห้ามทำซ้ำ
        if (isCompleted)
        {
            Debug.Log("ซ่อมเสร็จแล้ว");
            return;
        }

        // 🔍 เช็คของ
        foreach (var item in requiredItems)
        {
            if (!PlayerInventory.instance.Has(item.tag, item.amount))
            {
                Debug.Log("ยังไม่ครบ: " + item.tag);
                return;
            }
        }

        // ✔ หักของ
        foreach (var item in requiredItems)
        {
            PlayerInventory.instance.Remove(item.tag, item.amount);
        }

        // ✔ ตั้งสถานะว่าจบแล้ว
        isCompleted = true;

        Debug.Log("ซ่อมเรือสำเร็จ!");

        // 🚀 โหลด Scene ใหม่
        SceneManager.LoadScene(nextSceneName);
    }
}