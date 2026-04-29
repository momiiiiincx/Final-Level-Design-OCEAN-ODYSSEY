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

        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        if (col is BoxCollider2D box)
        {
            Gizmos.color = new Color(0f, 0.5f, 1f, 0.3f);
            Gizmos.DrawCube(box.offset, box.size);
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(box.offset, box.size);
            Gizmos.color = new Color(0f, 1f, 1f, 0.2f);
            Gizmos.DrawLine(box.offset - box.size / 2, box.offset + box.size / 2);
            Gizmos.DrawLine(box.offset + new Vector2(-box.size.x, box.size.y) / 2, box.offset + new Vector2(box.size.x, -box.size.y) / 2);
        }

        Gizmos.matrix = Matrix4x4.identity;
        Vector3 center = col.bounds.center;

        float arrowLength = Mathf.Max(2f, Mathf.Abs(waveForce) * 0.1f);
        Vector3 upDir = transform.up * Mathf.Sign(waveForce >= 0 ? 1 : -1);
        Vector3 endPoint = center + upDir * arrowLength;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(center, endPoint);

        float arrowSize = 1f;
        Vector3 right = transform.right;
        Gizmos.DrawLine(endPoint, endPoint - upDir * arrowSize + right * arrowSize);
        Gizmos.DrawLine(endPoint, endPoint - upDir * arrowSize - right * arrowSize);
    }
}