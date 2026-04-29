using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Movement")]
    public float speed = 3.5f;

    [Header("Vision")]
    public float visionRange = 7f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= visionRange)
        {
            ChasePlayer();
        }
        else
        {
            StopMoving();
        }
    }

    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;

        // เคลื่อนที่
        rb.linearVelocity = direction * speed;

        // หมุน
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = angle - 90f; // ปรับตาม sprite
    }

    void StopMoving()
    {
        rb.linearVelocity = Vector2.zero;
    }

    // 💀 ชน Player = Game Over
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene("GameOver");
        }
    }

    // 👀 วาดวงมองเห็น (debug)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, visionRange);
    }
}