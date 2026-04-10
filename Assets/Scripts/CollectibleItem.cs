using UnityEngine;
using UnityEngine.InputSystem;

public class CollectibleItem : MonoBehaviour
{
    public enum ItemType { Wood, Rope }
    public ItemType type;
    private bool isPlayerNearby = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) isPlayerNearby = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) isPlayerNearby = false;
    }

    private void Update()
    {
        if (isPlayerNearby && Keyboard.current.eKey.wasPressedThisFrame)
        {
            Collect();
        }
    }

    private void Collect()
    {
        QuestManager qm = FindObjectOfType<QuestManager>();
        if (qm != null && qm.isQuestActive && qm.questPhase == 1)
        {
            if (type == ItemType.Wood) qm.woodCount++;
            else if (type == ItemType.Rope) qm.ropeCount++;
            Destroy(gameObject);
        }
    }
}