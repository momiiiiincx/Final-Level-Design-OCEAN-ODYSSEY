using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class EnterNextLevel : MonoBehaviour
{
    [Header("Boat Controller (level 2 only)")]
    [SerializeField] private BoatController boatController;

    [Header("Fade Settings")]
    public Image fadeImage;
    public float fadeDuration = 1.5f;
    public string nextSceneName = "LevelName Here";

    private bool isFading = false;

    void Start()
    {
        if (boatController == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                boatController = player.GetComponent<BoatController>();
        }

        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0;
            fadeImage.color = c;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"[EnterNextLevel] Trigger hit by: {other.name} (tag: {other.tag})");
        if (other.CompareTag("Player") && !isFading)
        {
            if (boatController != null)
                boatController.enabled = false;

            Debug.Log("[EnterNextLevel] Player detected — loading scene: " + nextSceneName);
            StartCoroutine(FadeAndChangeScene());
        }
    }

    private IEnumerator FadeAndChangeScene()
    {
        isFading = true;

        if (fadeImage != null)
        {
            Color tempColor = fadeImage.color;
            tempColor.a = 1;
            fadeImage.color = tempColor;

            yield return new WaitForSeconds(1f);
        }

        SceneManager.LoadScene(nextSceneName);
    }
}
