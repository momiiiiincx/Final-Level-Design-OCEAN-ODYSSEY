using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class WaveSystem : MonoBehaviour
{
    [Header("Wave Settings")]
    public bool allowBoatController = false;
    public float waveForce;

    private BoatController _cachedBoat;
    private Rigidbody2D _cachedRb;

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (!other.TryGetComponent(out BoatController boat))
            boat = other.GetComponentInParent<BoatController>();

        if (boat == null) return;

        _cachedBoat = boat;

        if (!boat.TryGetComponent(out _cachedRb))
            _cachedRb = boat.GetComponentInParent<Rigidbody2D>();

        if (!allowBoatController)
            boat.enabled = false;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (_cachedBoat == null) return;

        if (_cachedRb != null)
        {
            if (!allowBoatController)
                _cachedRb.linearVelocity = (Vector2)transform.up * waveForce;
            else
                _cachedRb.AddForce((Vector2)transform.up * waveForce);
        }
        else _cachedBoat.transform.position += waveForce * Time.deltaTime * transform.up;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (_cachedBoat == null) return;

        _cachedBoat.enabled = true;
        if (_cachedRb != null) _cachedRb.linearVelocity = Vector2.zero;

        _cachedBoat = null;
        _cachedRb = null;
    }

    // เอาไว้ดูตำแหน่ง Trigger
    private void OnDrawGizmos()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col == null) return;

        Gizmos.color = new Color(0f, 0.5f, 1f, 0.2f);
        Gizmos.DrawCube(col.bounds.center, col.bounds.size);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);

        // Use local size so extents stay consistent regardless of Z rotation
        float halfW = col is BoxCollider2D box ? box.size.x * 0.5f : col.bounds.extents.x;
        float halfH = col is BoxCollider2D box2 ? box2.size.y * 0.5f : col.bounds.extents.y;

        Vector3 center = col.bounds.center;
        Vector3 right = transform.right;
        Vector3 up = transform.up;

        // Horizontal line across the wave surface (perpendicular to force)
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(center - right * halfW, center + right * halfW);

        // Arrow shaft + head pointing in wave force direction (transform.up)
        Vector3 arrowTip = center + up * halfH;
        Gizmos.DrawLine(center, arrowTip);

        float arrowSize = Mathf.Min(0.5f, halfH * 0.3f);
        Gizmos.DrawLine(arrowTip, arrowTip + (-up + right) * arrowSize);
        Gizmos.DrawLine(arrowTip, arrowTip + (-up - right) * arrowSize);
    }
}
