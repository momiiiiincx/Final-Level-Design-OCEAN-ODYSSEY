using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Visual References")]
    [SerializeField] private Transform visualTransform; // ลาก Object ตัวลูก (Sprite) มาใส่ที่นี่
    [SerializeField] private Animator anim;           // ลาก Animator ที่อยู่ในตัวลูกมาใส่ที่นี่

    [Header("Animation Names")]
    [SerializeField] private string idleAnimName = "Player_Idle";
    [SerializeField] private string runAnimName = "Player_Run";

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private string currentAnimation;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // ตั้งค่า Rigidbody ของตัวแม่
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void Update()
    {
        HandleAnimation();
        FlipSprite();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }

    private void HandleAnimation()
    {
        if (anim == null) return;

        string targetAnim = (moveInput.sqrMagnitude > 0) ? runAnimName : idleAnimName;

        if (currentAnimation != targetAnim)
        {
            anim.Play(targetAnim, 0, 0f);
            currentAnimation = targetAnim;
        }
    }

    private void FlipSprite()
    {
        if (visualTransform == null) return;

        // เปลี่ยน Scale เฉพาะตัวลูก (Visual)
        if (moveInput.x > 0.1f)
            visualTransform.localScale = new Vector3(1, 1, 1);
        else if (moveInput.x < -0.1f)
            visualTransform.localScale = new Vector3(-1, 1, 1);
    }
}