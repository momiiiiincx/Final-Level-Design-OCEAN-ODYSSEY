using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro; // สำคัญมาก: ต้องเพิ่มบรรทัดนี้เพื่อให้รู้จัก TextMeshPro

public class GuideController : MonoBehaviour
{
    [Header("Settings")]
    public float displayDuration = 2f; // ลองตั้ง 2 วินาทีเพื่อเทส
    public float fadeDuration = 2f;

    private Image panelImage;
    private TextMeshProUGUI guideText; // เปลี่ยนจาก Text เป็น TextMeshProUGUI

    void Start()
    {
        panelImage = GetComponent<Image>();
        // เปลี่ยนมาหา Component ของ TextMeshPro
        guideText = GetComponentInChildren<TextMeshProUGUI>();

        if (panelImage == null) Debug.LogError("หา Image ไม่เจอจ้า! ตรวจสอบว่าใส่สคริปต์ถูกที่ไหม");
        if (guideText == null) Debug.LogError("หา TextMeshPro ไม่เจอจ้า! ตรวจสอบว่าตัวหนังสือเป็น TMP หรือเปล่า");

        if (panelImage != null && guideText != null)
        {
            Debug.Log("เริ่มนับถอยหลัง " + displayDuration + " วินาที...");
            StartCoroutine(StartGuideProcess());
        }
    }

    IEnumerator StartGuideProcess()
    {
        yield return new WaitForSeconds(displayDuration);

        float currentTime = 0;
        Color pColor = panelImage.color;
        Color tColor = guideText.color;

        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, currentTime / fadeDuration);

            panelImage.color = new Color(pColor.r, pColor.g, pColor.b, alpha);
            guideText.color = new Color(tColor.r, tColor.g, tColor.b, alpha);

            yield return null;
        }

        gameObject.SetActive(false);
    }
} 