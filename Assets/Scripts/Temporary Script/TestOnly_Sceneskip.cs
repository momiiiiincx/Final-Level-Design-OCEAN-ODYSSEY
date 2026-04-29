using UnityEngine;

public class TestOnly_Sceneskip : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "NextScene";

    [ContextMenu("Skip Scene")]
    private void SkipScene()
    {
        Debug.Log("Skipping to scene: " + nextSceneName);
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
    }

    [Header("Script Settings")]
    [Tooltip("เปิดใช้มั้ย")]
    public bool enableSceneSkip = false;

    [Header("Components")]
    [Tooltip("เอาไว้กรณีที่อยากให้ผู้เล่นเหยียบ สำหรับ Test เท่านั้น อย่าหาเอาไปใส่มั่ว")]
    [SerializeField] private Collider2D triggerCollider;

    void Awake()
    {
        triggerCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && enableSceneSkip) SkipScene();
    }
}