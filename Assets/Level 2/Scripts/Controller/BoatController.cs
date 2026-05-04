using UnityEngine;
using UnityEngine.InputSystem;

public class BoatController : MonoBehaviour
{
    [Header("Control Settings")]
    [SerializeField] private float _speed;
    
    // 👇 เพิ่มบรรทัดนี้: ให้ตั้งค่าตัวคูณความเร็วใน Inspector ได้
    [Tooltip("ตัวคูณความเร็วเมื่อโดนน้ำวน (เช่น 2 = เร็วขึ้น 2 เท่า)")]
    [SerializeField] private float _whirlpoolSpeedMultiplier = 2f; 

    [Space(10)]
    [Header("references")]
    [SerializeField] private Transform _boatTransform;
    [SerializeField] private Rigidbody2D _rb2d;

    [Header("Sprite Renderers")]
    [SerializeField] private SpriteRenderer _boatRenderer;
    [SerializeField] private Sprite _idleSprite;
    [SerializeField] private Sprite _movingSprite;

    private Vector2 _moveInput;
    private const float MOVE_THRESHOLD = 0.01f;

    // 👇 เพิ่มบรรทัดนี้: เก็บค่าความเร็วเดิมไว้
    private float _baseSpeed; 

    private void Start()
    {
        // 👇 เก็บค่า Speed ปกติไว้ตอนเริ่มเกม
        _baseSpeed = _speed; 
    }

    public void OnMovement(InputValue value) => _moveInput = value.Get<Vector2>();
    public void SetMovement(Vector2 input) => _moveInput = input;

    private void FixedUpdate()
    {
        Vector2 move = _moveInput;
        
        // (ผมยังเก็บส่วน Input แบบเก่าไว้ให้ตามโค้ดเดิมของคุณนะครับ)
        if (move.sqrMagnitude < MOVE_THRESHOLD)
        {
            move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }

        if (move.sqrMagnitude > MOVE_THRESHOLD)
        {
            if (_rb2d != null)
            {
                _rb2d.linearVelocity = move.normalized * _speed;
            }

            if (_boatTransform != null)
            {
                if (_boatRenderer != null)
                {
                    if (Mathf.Abs(move.x) > MOVE_THRESHOLD)
                    {
                        _boatRenderer.flipX = move.x < 0;
                    }
                }
            }

            if (_boatRenderer != null && _movingSprite != null)
                _boatRenderer.sprite = _movingSprite;
        }
        else
        {
            if (_rb2d != null)
            {
                _rb2d.linearVelocity = Vector2.zero;
            }

            if (_boatRenderer != null && _idleSprite != null)
                _boatRenderer.sprite = _idleSprite;
        }
    }

    // 👇 เพิ่มระบบชนน้ำวนตรงนี้
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Whirlpool"))
        {
            // เอาความเร็วเริ่มต้น มาคูณกับตัวเลขที่คุณตั้งใน Inspector
            _speed = _baseSpeed * _whirlpoolSpeedMultiplier; 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Whirlpool"))
        {
            // ออกจากน้ำวนแล้ว ให้กลับมาใช้ความเร็วปกติ
            _speed = _baseSpeed; 
        }
    }
}