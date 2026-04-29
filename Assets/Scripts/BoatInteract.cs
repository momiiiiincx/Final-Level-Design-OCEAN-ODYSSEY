using UnityEngine;

public class BoatInteract : MonoBehaviour
{
    public BoatRepair boat;
    public GameObject interactUI; // 👈 UI ที่จะโชว์

    private bool isPlayerNear = false;

    void Update()
    {
        if (isPlayerNear)
        {
            // เปิด UI
            if (interactUI != null)
                interactUI.SetActive(true);

            // กด E
            if (Input.GetKeyDown(KeyCode.E))
            {
                boat.TryRepair();
            }
        }
        else
        {
            // ปิด UI
            if (interactUI != null)
                interactUI.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
        }
    }
}