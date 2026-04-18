using UnityEngine;
using System.Collections;

public class SheepAI : MonoBehaviour
{
    public enum State { Idle, Walking, Eating }

    [Header("Movement Settings")]
    public Transform pointA;
    public Transform pointB;
    public float moveSpeed = 2f;
    public float waitTime = 2f;

    [Header("Animation States")]
    private const string ANIM_RUN = "Sheep_Run";
    private const string ANIM_IDLE = "Sheep_Idle";
    private const string ANIM_GRASS = "Sheep_Grass";

    private Animator anim;
    private Transform targetPoint;
    private State currentState;

    void Start()
    {
        anim = GetComponent<Animator>();
        targetPoint = pointB;

        // ตรวจสอบว่ามี Point ครบไหมป้องกัน Error
        if (pointA == null || pointB == null)
        {
            Debug.LogError("กรุณาลาก Point A และ B มาใส่ใน Inspector ด้วยครับ!");
            return;
        }

        StartCoroutine(SheepBehavior());
    }

    IEnumerator SheepBehavior()
    {
        while (true)
        {
            // 1. เดินไปที่เป้าหมาย (Run)
            currentState = State.Walking;
            anim.Play(ANIM_RUN);
            FlipSprite();

            while (Vector2.Distance(transform.position, targetPoint.position) > 0.1f)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);
                yield return null;
            }

            // 2. เมื่อถึงจุดหมาย ให้หยุดนิ่ง (Idle)
            currentState = State.Idle;
            anim.Play(ANIM_IDLE);
            yield return new WaitForSeconds(waitTime);

            // 3. เริ่มกินหญ้า (Grass)
            currentState = State.Eating;
            anim.Play(ANIM_GRASS);
            yield return new WaitForSeconds(waitTime * 2f);

            // 4. สลับจุดหมายเพื่อเตรียมเดินครั้งต่อไป
            targetPoint = (targetPoint == pointA) ? pointB : pointA;

            yield return null;
        }
    }

    void FlipSprite()
    {
        if (targetPoint.position.x > transform.position.x)
        {
            // หันขวา
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            // หันซ้าย
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
}