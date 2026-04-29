using UnityEngine;

public class PlayerAwarenessController : MonoBehaviour
{
    public float detectionRadius = 5f;
    public LayerMask playerLayer;

    public bool AwareOfPlayer { get; private set; }
    public Vector2 DirectionToPlayer { get; private set; }

    void Update()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);

        if (player != null)
        {
            AwareOfPlayer = true;
            DirectionToPlayer = (player.transform.position - transform.position).normalized;
        }
        else
        {
            AwareOfPlayer = false;
            DirectionToPlayer = Vector2.zero;
        }
    }
}
