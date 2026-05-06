using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [Header("Required Items")]
    public int requiredWood = 2;
    public int requiredRope = 1;

    [Header("Scene Settings")]
    public string nextScene;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (Inventory.instance.woodCount >= requiredWood &&
            Inventory.instance.ropeCount >= requiredRope)
        {
            SceneManager.LoadScene(nextScene);
        }
        else
        {
            Debug.Log("เก็บของไม่ครบ! ไม้: " + Inventory.instance.woodCount + "/" + requiredWood
                    + " เชือก: " + Inventory.instance.ropeCount + "/" + requiredRope);
        }
    }
}