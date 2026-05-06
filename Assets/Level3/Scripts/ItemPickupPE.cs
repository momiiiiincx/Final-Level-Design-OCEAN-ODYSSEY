using UnityEngine;

public enum ItemType { Wood, Rope }

public class ItemPickupPE : MonoBehaviour
{
    [Header("Item Settings")]
    public ItemType itemType;
    public float pickupRange = 2f;

    private Transform player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= pickupRange && Input.GetKeyDown(KeyCode.E))
        {
            PickupItem();
        }
    }

    void PickupItem()
    {
        Inventory.instance.AddItem(itemType);
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}