using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionSceneChanger : MonoBehaviour
{
    [Header("Scene Settings")]
    public string sceneName;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
