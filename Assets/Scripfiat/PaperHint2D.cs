using UnityEngine;

public class PaperHint2D : MonoBehaviour
{
    public GameObject hintUI; // ลาก UI Image มาใส่
    private bool isNear = false;
    private bool isOpen = false;

    void Update()
    {
        // กด E เพื่อเปิด
        if (isNear && Input.GetKeyDown(KeyCode.E) && !isOpen)
        {
            hintUI.SetActive(true);
            isOpen = true;

            Time.timeScale = 0f; // หยุดเกม (ไม่อยากหยุดก็ลบได้)
        }

        // กด ESC เพื่อปิด
        if (isOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            hintUI.SetActive(false);
            isOpen = false;

            Time.timeScale = 1f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isNear = false;
        }
    }
}
