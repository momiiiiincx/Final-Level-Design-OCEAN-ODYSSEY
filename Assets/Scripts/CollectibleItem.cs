using UnityEngine;
using UnityEngine.InputSystem;

public class CollectibleItem : MonoBehaviour
{
    public enum ItemType { Wood, Rope }
    public ItemType type;
    private bool isPlayerNearbyItem = false;

    private void OnTriggerEnter2D(Collider2D other) { if (other.CompareTag("Player")) isPlayerNearbyItem = true; }
    private void OnTriggerExit2D(Collider2D other) { if (other.CompareTag("Player")) isPlayerNearbyItem = false; }

    private void Update()
    {
        if (isPlayerNearbyItem && Keyboard.current.eKey.wasPressedThisFrame)
        {
            QuestManager qm = Object.FindFirstObjectByType<QuestManager>();
            if (qm != null && qm.questPhase == 1)
            {
                if (type == ItemType.Wood) qm.woodCount++; else qm.ropeCount++;
                Destroy(gameObject);
            }
        }
    }
}