using UnityEngine;

public class GREENBOTTON : MonoBehaviour
{
   public GameObject codeUI;
   public CodeSystem codeSystem; // Panel ใส่รหัส

    private bool playerInRange = false;
    public ObjectiveManager objectiveManager;

   void Update()
{
    if (playerInRange && Input.GetKeyDown(KeyCode.E))
    {
        codeSystem.OpenUI();
         objectiveManager.SetObjective("Go to the red circle"); 
    }
}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("เข้าใกล้ปุ่ม");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
