using UnityEngine;

public class Shooting : MonoBehaviour
{
    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletForce = 10f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);

        // 🔥 ใส่ logic ให้กระสุน
        bullet.AddComponent<Bullet>();

        // 💥 ลบกระสุนใน 10 วิ
        Destroy(bullet, 10f);
    }
}

// -----------------------------

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject); // ลบ Enemy
            Destroy(gameObject);           // ลบกระสุน
        }
    }
}
