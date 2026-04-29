using UnityEngine;

public class RedCircleGoal : MonoBehaviour
{
    private bool isUnlocked = false;

    public void Unlock()
    {
        isUnlocked = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isUnlocked) return; // 🔥 กันก่อนปลดล็อก

        if (other.CompareTag("Player"))
        {
            Debug.Log("เข้าวงแดงแล้ว");

            Time.timeScale = 0f;
        }
    }
}