using UnityEngine;

public class FogPingPong : MonoBehaviour
{
    public float speed = 1f;     // ความเร็ว
    public float distance = 1f;  // ระยะไปกลับ

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * speed) * distance;
        transform.position = startPos + new Vector3(offset, 0, 0);
    }
}