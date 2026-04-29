using UnityEngine;
using TMPro;

public class StartTextFade : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float delay = 2f;     // อยู่ค้างก่อนจาง
    public float fadeSpeed = 1f; // ความเร็ว fade

    private float timer = 0f;
    private Color color;

    void Start()
    {
        color = text.color;
    }

    void Update()
    {
        timer += Time.deltaTime;

        // รอ delay ก่อน
        if (timer > delay)
        {
            color.a -= Time.deltaTime * fadeSpeed;
            text.color = color;

            // ถ้าจางหมด → ปิด object
            if (color.a <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}