using UnityEngine;

public class Bote : MonoBehaviour
{
    public GameObject hintUI;
    public string message = "น้ำไหลขึ้นด้านบน... ลองตามกระแสดู";

    public GameObject pressEText;

    private bool playerInRange = false;
    public ObjectiveManager objectiveManager;

    void Start()
    {
        pressEText.SetActive(false); // 🔥 ปิดตั้งแต่เริ่ม
        hintUI.SetActive(false);     // 🔥 กัน UI ค้าง
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            hintUI.SetActive(!hintUI.activeSelf);
            objectiveManager.SetObjective("Press the green button");

            // 🔥 ถ้าเปิด UI → ซ่อน Press E
            if (hintUI.activeSelf)
                pressEText.SetActive(false);
            else
                pressEText.SetActive(true);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            pressEText.SetActive(true);
            Debug.Log("เข้าแล้ว");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            pressEText.SetActive(false);
            hintUI.SetActive(false);
        }
    }
}
