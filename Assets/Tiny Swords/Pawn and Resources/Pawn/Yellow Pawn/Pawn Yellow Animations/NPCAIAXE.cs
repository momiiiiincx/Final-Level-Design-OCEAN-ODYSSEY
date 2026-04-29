using UnityEngine;
using System.Collections;

public class NPCAIAXE : MonoBehaviour
{
    public enum State { Idle, Walking, Interacting }

    [Header("Movement Settings")]
    public Transform pointA;
    public Transform pointB;
    public float moveSpeed = 2f;

    [Header("Animation States")]
    // เปลี่ยนชื่อท่าตามที่กำหนด
    private const string ANIM_RUN = "Pawn_Run Axe_Yellow";
    private const string ANIM_IDLE = "Pawn_Idle Axe_Yellow";
    private const string ANIM_INTERACT = "Pawn_Interact Axe_Yellow";

    private Animator anim;
    private Transform targetPoint;
    private State currentState;

    void Start()
    {
        anim = GetComponent<Animator>();
        targetPoint = pointB;

        if (pointA == null || pointB == null)
        {
            Debug.LogError("กรุณาลาก Point A และ B มาใส่ใน Inspector ด้วยครับ!");
            return;
        }

        StartCoroutine(NPCBehavior());
    }

    IEnumerator NPCBehavior()
    {
        while (true)
        {
            // 1. เดินไปที่เป้าหมาย (Run)
            currentState = State.Walking;
            anim.Play(ANIM_RUN);

            // เช็คทิศทางก่อนเดิน
            FlipSprite();

            while (Vector2.Distance(transform.position, targetPoint.position) > 0.1f)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);
                yield return null;
            }

            // 2. เมื่อถึงจุดหมาย ให้หยุดนิ่ง (Idle)
            currentState = State.Idle;
            anim.Play(ANIM_IDLE);

            // สุ่มเวลารอ 1-2 วินาที
            float waitIdle = Random.Range(1f, 2f);
            yield return new WaitForSeconds(waitIdle);

            // 3. ทำท่า Interact (เช่น ขุดดิน หรือทำงาน)
            currentState = State.Interacting;
            anim.Play(ANIM_INTERACT);

            // สุ่มเวลาทำงาน 1-2 วินาที
            float waitInteract = Random.Range(1f, 2f);
            yield return new WaitForSeconds(waitInteract);

            // 4. สลับจุดหมาย
            targetPoint = (targetPoint == pointA) ? pointB : pointA;

            yield return null;
        }
    }

    void FlipSprite()
    {
        // คำนวณการหันหน้าตามแกน X
        if (targetPoint.position.x > transform.position.x)
        {
            // หันขวา
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
        }
        else
        {
            // หันซ้าย
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
        }
    }
}