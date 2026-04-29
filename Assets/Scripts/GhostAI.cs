using UnityEngine;

// บรรทัดนี้จะบังคับให้ Unity เช็คว่าตัวผีมี Rigidbody2D หรือไม่ (กันลืมใส่)
[RequireComponent(typeof(Rigidbody2D))] 
public class GhostAI : MonoBehaviour
{
    [Header("ตั้งค่าความเร็ว")]
    public float chaseSpeed = 3.5f;   
    public float patrolSpeed = 1.5f;  
    
    [Header("ระบบการมองเห็น (สายตาผี)")]
    public float viewDistance = 8f;        
    public LayerMask obstacleLayer;        

    [Header("ระบบทำลายตู้")]
    public float destroyDistance = 0.8f; 
    public float timeBetweenDestruction = 15f; 
    
    [Header("ระบบเดินสุ่ม (Wander)")]
    public float wanderRadius = 4f;        
    private Vector2 randomDestination;     
    private float stuckTimer = 0f;         

    [Header("ระบบชนกำแพง (หยุดชะงัก)")]
    public float wallPauseTime = 1f;       
    private bool isPausing = false;        
    private float currentPauseTimer = 0f;

    // ตัวแปรควบคุมการพังตู้
    private float cooldownTimer = 0f;
    private bool isWaitingForDestroy = true; 

    private Transform playerTransform;
    private PlayerHide playerHideScript;
    private Transform targetCabinet;     
    
    // ตัวแปรฟิสิกส์ของผี
    private Rigidbody2D rb; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // ดึงคอมโพเนนต์มาเก็บไว้

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
            playerHideScript = playerObj.GetComponent<PlayerHide>();
        }
        SetNewRandomDestination();
    }

    void FixedUpdate()
    {
        if (playerTransform == null || playerHideScript == null) return;

        // --- ระบบหยุดพักเมื่อชนกำแพง ---
        if (isPausing)
        {
            currentPauseTimer -= Time.deltaTime;
            if (currentPauseTimer <= 0f)
            {
                isPausing = false; 
                SetNewRandomDestination(); // สุ่มหาจุดเดินใหม่ จะได้ไม่หันไปไถกำแพงเดิม
            }
            return; 
        }

        if (CanSeePlayer())
        {
            ChasePlayer();
        }
        else
        {
            HandleWanderAndDestroyLogic();
        }
    }

    bool CanSeePlayer()
    {
        if (playerHideScript.isHiding) return false;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer > viewDistance) return false;

        Vector2 direction = (playerTransform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distanceToPlayer, obstacleLayer);

        return hit.collider == null; 
    }

    void ChasePlayer()
    {
        targetCabinet = null;
        isWaitingForDestroy = true; 
        
        // 🚨 เปลี่ยนมาใช้ rb.MovePosition เพื่อไม่ให้ทะลุกำแพง
        Vector2 nextPos = Vector2.MoveTowards(rb.position, playerTransform.position, chaseSpeed * Time.deltaTime);
        rb.MovePosition(nextPos);
    }

    void HandleWanderAndDestroyLogic()
    {
        if (isWaitingForDestroy)
        {
            HandleWandering(); 
            
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= timeBetweenDestruction)
            {
                isWaitingForDestroy = false; 
                cooldownTimer = 0f;
                FindNewCabinetTarget();      
            }
        }
        else
        {
            if (targetCabinet == null)
            {
                isWaitingForDestroy = true;
                return;
            }

            // 🚨 เปลี่ยนมาใช้ rb.MovePosition
            Vector2 nextPos = Vector2.MoveTowards(rb.position, targetCabinet.position, patrolSpeed * Time.deltaTime);
            rb.MovePosition(nextPos);

            if (Vector2.Distance(transform.position, targetCabinet.position) <= destroyDistance)
            {
                DestroyTargetCabinet();
            }
        }
    }

    void DestroyTargetCabinet()
    {
        if (playerHideScript.isHiding && playerHideScript.currentCabinet == targetCabinet.gameObject)
        {
            Debug.Log("โดนจับได้! ผู้เล่นกระเด็นออกจากตู้");
            if (GameManagerGhost.Instance != null)
            {
                GameManagerGhost.Instance.GameOver(); 
            }
            playerHideScript.ForceUnhideFromGhost(); 
        }

        Destroy(targetCabinet.gameObject);

        targetCabinet = null;
        isWaitingForDestroy = true;
        cooldownTimer = 0f;
        SetNewRandomDestination();
    }

    void HandleWandering()
    {
        // 🚨 เปลี่ยนมาใช้ rb.MovePosition
        Vector2 nextPos = Vector2.MoveTowards(rb.position, randomDestination, patrolSpeed * Time.deltaTime);
        rb.MovePosition(nextPos);

        stuckTimer += Time.deltaTime;

        // ถ้าเดินใกล้ถึงเป้าหมาย หรือเดินติดกำแพงนานกว่า 3 วิ ให้สุ่มเป้าหมายใหม่
        if (Vector2.Distance(transform.position, randomDestination) < 0.2f || stuckTimer > 3f)
        {
            SetNewRandomDestination();
        }
    }

    void SetNewRandomDestination()
    {
        Vector2 randomDirection = Random.insideUnitCircle * wanderRadius;
        randomDestination = (Vector2)transform.position + randomDirection;
        stuckTimer = 0f;
    }

    void FindNewCabinetTarget()
    {
        GameObject[] cabinets = GameObject.FindGameObjectsWithTag("Cabinet");
        if (cabinets.Length > 0)
        {
            int randomIndex = Random.Range(0, cabinets.Length);
            targetCabinet = cabinets[randomIndex].transform;
        }
        else
        {
            isWaitingForDestroy = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ถ้าสิ่งที่ชนคือ Player
        if (collision.gameObject.CompareTag("Player"))
        {
            if (GameManagerGhost.Instance != null)
            {
                GameManagerGhost.Instance.GameOver();
            }
        }
        
        // ถ้าสิ่งที่ชนคือ กำแพง (Obstacle)
        else if (((1 << collision.gameObject.layer) & obstacleLayer) != 0)
        {
            isPausing = true;                    
            currentPauseTimer = wallPauseTime;   
            randomDestination = transform.position; // ล้างจุดหมายเก่าทิ้งทันที
        }
    }
}