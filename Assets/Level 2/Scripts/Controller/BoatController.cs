using UnityEngine;
using UnityEngine.InputSystem;

public class BoatController : MonoBehaviour
{
    // TODO: --> แก้ script ติดต่อไอเปรียว <--

    [Header("Control Settings")]
    // TODO: (Optional) จะเอา SerializeField ออกตอน Product หรือว่าไม่ก็ได้ (ถ้าเอาออกต้องเปลี่ยน private เป็น public ด้วย)
    [SerializeField] private float _speed;

    [Space(10)]
    [Header("references")]
    [SerializeField] private Transform _boatTransform;
    [SerializeField] private Rigidbody2D _rb2d;

    [Header("Sprite Renderers")] // TODO: อย่าหาแก้ ถ้าจะทำ animation ให้ทำเป็น animator แทน
    [SerializeField] private SpriteRenderer _boatRenderer;
    [SerializeField] private Sprite _idleSprite;
    [SerializeField] private Sprite _movingSprite;

    private Vector2 _moveInput;
    private const float MOVE_THRESHOLD = 0.01f;

    // TODO: (Optional) ถ้าอยากเปลี่ยนไปใช้ script ที่ใช้เคลื่อนที่ก็เอาส่วนนี้ออก แต่ไม่แนะนำ
    // Simplify ให้แล้วใช้ Input System แทน
    // แปลว่าถ้าอยากเปลี่ยนไปใช่ script ที่ใช้เคลื่อนที่ก็เอาส่วนนี้ออก
    public void OnMovement(InputValue value) => _moveInput = value.Get<Vector2>();
    public void SetMovement(Vector2 input) => _moveInput = input;

    private void FixedUpdate()
    {
        Vector2 move = _moveInput;
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
                        // ตัว main sprite เรือหันทางขวา
                        // ถ้าเรือหันทางซ้ายก็เอา flipX ออกแล้วเปลี่ยน targetZ เป็น 180f หรือ -180f
                        _boatRenderer.flipX = move.x < 0;
                    }
                }

                // TODO: ถ้าอยากได้เรือหันขึ้นลง ก็เอา Comment ออก

                // float targetZ = 0f;
                // if (Mathf.Abs(move.y) > Mathf.Abs(move.x))
                // {
                //     targetZ = move.y > 0 ? 90f : -90f;

                //     if (_boatRenderer != null && _boatRenderer.flipX)
                //     {
                //         targetZ = -targetZ;
                //     }
                // }

                // _boatTransform.rotation = Quaternion.Euler(0f, 0f, targetZ);
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
}
