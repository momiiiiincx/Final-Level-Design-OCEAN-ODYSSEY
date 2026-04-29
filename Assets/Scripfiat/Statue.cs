using UnityEngine;

public class Statue : MonoBehaviour
{
    public int statueID;
    public StatuePuzzle puzzleManager;

    private bool playerInRange = false;

    private SpriteRenderer sr;
    private Color originalColor;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            puzzleManager.PressStatue(this);
        }
    }

    public void SetGreen()
    {
        sr.color = Color.green;
    }

    public void SetRed()
    {
        sr.color = Color.red;
    }

    public void ResetColor()
    {
        sr.color = originalColor;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}