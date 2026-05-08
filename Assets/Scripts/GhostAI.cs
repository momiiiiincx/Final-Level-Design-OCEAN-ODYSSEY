using UnityEngine;

// บังคับให้ Unity เช็คว่ามี Rigidbody2D และ AudioSource หรือไม่
[RequireComponent(typeof(Rigidbody2D))] 
[RequireComponent(typeof(AudioSource))] 
public class GhostAI : MonoBehaviour
{
    [Header("ตั้งค่าความเร็ว")]
    public float chaseSpeed = 3.5f;   
    public float patrolSpeed = 1.5f;  
    
    [Header("ระบบการมองเห็น (สายตาผีมองทะลุกำแพง)")]
    public float viewDistance = 8f;        
    public LayerMask obstacleLayer; // ยังต้องใช้สำหรับเช็คตอนเดินชนกำแพง

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

    [Header("ระบบเสียงผี")]
    public AudioClip ghostSound;       
    [Range(0f, 1f)]
    public float maxVolume = 1f;       // ความดังสูงสุดเมื่ออยู่ใกล้สุด
    public float minHearingDistance = 2f;  // ระยะที่เสียงจะดังที่สุด (เข้ามาใกล้กว่านี้ก็ดังเท่าเดิม)
    public float maxHearingDistance = 12f; // ระยะที่เสียงจะเบาจนดับไปเลย (ไกลกว่านี้คือไม่ได้ยิน)

    // ตัวแปรควบคุมการพังตู้
    private float cooldownTimer = 0f;
    private bool isWaitingForDestroy = true; 

    private Transform playerTransform;
    private PlayerHide playerHideScript;
    private Transform targetCabinet;     
    
    // ตัวแปรฟิสิกส์และเสียง
    private Rigidbody2D rb; 
    private AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
        audioSource = GetComponent<AudioSource>(); 

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
            playerHideScript = playerObj.GetComponent<PlayerHide>();
        }

        // ตั้งค่าเสียง
        if (ghostSound != null)
        {
            audioSource.clip = ghostSound;
            audioSource.loop = true; 
            audioSource.playOnAwake = false; 
            audioSource.volume = 0f; // เริ่มต้นให้เสียงเป็น 0 ไว้ก่อน จะได้ไม่ดังโพล่งขึ้นมา
            audioSource.Play();
        }

        SetNewRandomDestination();
    }

    // เพิ่มฟังก์ชัน Update สำหรับคำนวณระยะทางเสียงแบบเรียลไทม์
    void Update()
    {
        if (playerTransform != null && ghostSound != null)
        {
            // หาระยะห่างระหว่างผีกับผู้เล่น
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

            // คำนวณความดัง: ถ้าอยู่ใกล้กว่า minHearingDistance จะได้ค่า 1 (ดังสุด)
            // ถ้าอยู่ไกลกว่า maxHearingDistance จะได้ค่า 0 (ไม่ได้ยิน)
            float volumeMultiplier = 1f - Mathf.Clamp01((distanceToPlayer - minHearingDistance) / (maxHearingDistance - minHearingDistance));

            // นำตัวคูณมาคูณกับความดังสูงสุดที่เราตั้งไว้
            audioSource.volume = maxVolume * volumeMultiplier;
        }
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
                SetNewRandomDestination(); 
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
        return distanceToPlayer <= viewDistance; 
    }

    void ChasePlayer()
    {
        targetCabinet = null;
        isWaitingForDestroy = true; 
        
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
        Vector2 nextPos = Vector2.MoveTowards(rb.position, randomDestination, patrolSpeed * Time.deltaTime);
        rb.MovePosition(nextPos);

        stuckTimer += Time.deltaTime;

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
        if (collision.gameObject.CompareTag("Player"))
        {
            if (GameManagerGhost.Instance != null)
            {
                GameManagerGhost.Instance.GameOver();
            }
        }
        else if (((1 << collision.gameObject.layer) & obstacleLayer) != 0)
        {
            isPausing = true;                    
            currentPauseTimer = wallPauseTime;   
            randomDestination = transform.position; 
        }
    }
}