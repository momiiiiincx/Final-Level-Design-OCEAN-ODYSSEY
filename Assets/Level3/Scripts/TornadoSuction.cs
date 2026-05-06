using UnityEngine;

public class TornadoSuction : MonoBehaviour
{
    [Header("Suction Settings")]
    public float suctionForce = 10f;
    public float rotateForce = 5f;
    public float pullRadius = 5f;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.attachedRigidbody == null) return;

        Rigidbody2D rb = other.attachedRigidbody;
        Vector2 direction = (transform.position - other.transform.position).normalized;
        float distance = Vector2.Distance(transform.position, other.transform.position);

        // ยิ่งใกล้ยิ่งดูดแรง
        float forceMagnitude = suctionForce * (1 - distance / pullRadius);

        // ดูดเข้าแกนกลาง
        rb.AddForce(direction * forceMagnitude);

        // หมุนรอบ
        Vector2 perpendicular = new Vector2(-direction.y, direction.x);
        rb.AddForce(perpendicular * rotateForce);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, pullRadius);
    }
}
