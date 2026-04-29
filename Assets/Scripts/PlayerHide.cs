using UnityEngine;

public class PlayerHide : MonoBehaviour
{
    [Header("สถานะการซ่อนตัว")]
    public bool isHiding = false;      
    private bool isNearCabinet = false; 

    [Header("ตู้ที่กำลังใช้งาน")]
    public GameObject currentCabinet; // ตัวแปรนี้จะจำว่าผู้เล่นซ่อนอยู่/ยืนใกล้ตู้ใบไหน

    private SpriteRenderer spriteRenderer;
    private Character movementScript;    

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        movementScript = GetComponent<Character>(); 
    }

    void Update()
    {
        // ป้องกัน Error กรณีไม่ได้ซ่อนตัวอยู่ แต่ตู้ที่ยืนใกล้ๆ โดนผีพังไปแล้ว
        if (currentCabinet == null && !isHiding) 
        {
            isNearCabinet = false;
        }

        if (isNearCabinet && Input.GetKeyDown(KeyCode.E) && currentCabinet != null)
        {
            if (!isHiding)
            {
                Hide(); 
            }
            else
            {
                Unhide(); 
            }
        }
    }

    void Hide()
    {
        isHiding = true;
        spriteRenderer.enabled = false; 
        if (movementScript != null) movementScript.enabled = false;
        Debug.Log("ซ่อนตัวแล้ว!");
    }

    public void Unhide() // ต้องเปิดเป็น public เพื่อให้ผีเรียกใช้ได้ด้วย
    {
        isHiding = false;
        spriteRenderer.enabled = true; 
        if (movementScript != null) movementScript.enabled = true;
        Debug.Log("ออกจากตู้แล้ว!");
    }

    // ฟังก์ชันนี้ผีจะเรียกใช้ เมื่อมันพังตู้ที่เราซ่อนอยู่พอดี
    public void ForceUnhideFromGhost()
    {
        if (isHiding)
        {
            Unhide();
            currentCabinet = null;
            isNearCabinet = false;
            Debug.Log("ตู้พัง! ผู้เล่นร่วงออกมา!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Cabinet"))
        {
            isNearCabinet = true;
            currentCabinet = collision.gameObject; // จำตู้ที่เดินเข้าไปใกล้
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Cabinet"))
        {
            // ถ้าเดินออกจากตู้ และไม่ได้ซ่อนอยู่ ให้เคลียร์ค่าตู้
            if (!isHiding) 
            {
                isNearCabinet = false;
                currentCabinet = null;
            }
        }
    }
}