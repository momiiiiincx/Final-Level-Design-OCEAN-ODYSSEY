using UnityEngine;

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 200f;

    void Update()
    {
        float move = Input.GetAxis("Vertical");
        float rotate = Input.GetAxis("Horizontal");

        // เดินหน้า-ถอยหลัง
        transform.Translate(Vector2.up * move * speed * Time.deltaTime);

        // หมุนเรือ
        transform.Rotate(Vector3.forward * -rotate * rotationSpeed * Time.deltaTime);
    }

    public Vector2 moveDirection { get; private set; }

}


