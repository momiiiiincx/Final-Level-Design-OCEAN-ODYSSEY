using UnityEngine;
using System.Collections;

public class NPC_MinerAI : MonoBehaviour
{
    public enum State { Idle, Walking, Mining }

    [Header("Movement Settings")]
    public Transform pointA;
    public Transform pointB;
    public float moveSpeed = 2f;

    [Header("Animation States")]
    // เปลี่ยนชุดท่าทางเป็นแบบถือ Pickaxe (จอบ/อีเต้อ)
    private const string ANIM_RUN = "Pawn_Run Pickaxe_Yellow";
    private const string ANIM_IDLE = "Pawn_Idle Pickaxe_Yellow";
    private const string ANIM_MINING = "Pawn_Interact Pickaxe_Yellow";

    private Animator anim;
    private Transform targetPoint;
    private State currentState;

    void Start()
    {
        anim = GetComponent<Animator>();
        targetPoint = pointB;

        if (pointA == null || pointB == null)
        {
            Debug.LogError("กรุณาลาก Point A และ B มาใส่ใน Miner NPC ด้วยครับ!");
            return;
        }

        StartCoroutine(MinerRoutine());
    }

    IEnumerator MinerRoutine()
    {
        while (true)
        {
            // 1. เดินไปที่จุดหมาย (Run)
            currentState = State.Walking;
            anim.Play(ANIM_RUN);
            FlipSprite();

            while (Vector2.Distance(transform.position, targetPoint.position) > 0.1f)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);
                yield return null;
            }

            // 2. เมื่อถึงจุดหมาย ให้หยุดพัก (Idle)
            currentState = State.Idle;
            anim.Play(ANIM_IDLE);

            // สุ่มเวลารอ 1-2 วินาที
            yield return new WaitForSeconds(Random.Range(1f, 2f));

            // 3. เริ่มขุดเหมือง (Interact/Mining)
            currentState = State.Mining;
            anim.Play(ANIM_MINING);

            // สุ่มเวลาขุด 1-2 วินาที
            yield return new WaitForSeconds(Random.Range(1f, 2f));

            // 4. สลับจุดหมายเพื่อเดินกลับ
            targetPoint = (targetPoint == pointA) ? pointB : pointA;

            yield return null;
        }
    }

    void FlipSprite()
    {
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