using UnityEngine;
using UnityEngine.SceneManagement; 
using System.Collections; 

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))] 
public class OctopusAI : MonoBehaviour
{
    [Header("ตั้งค่าปลาหมึก (ไล่ล่า)")]
    public float chaseSpeed = 4f;     
    private bool isChasing = true;    

    [Header("การเปลี่ยน Scene")]
    public string retrySceneName = "RetryScene"; 

    [Header("ตั้งค่าเสียงเมื่อเข้าใกล้")]
    public AudioClip approachSound;        
    public float soundTriggerDistance = 5f; 
    private AudioSource audioSource;
    private bool hasPlayedSound = false;   

    [Header("ระบบโจมตีด้วยหนวดจากใต้น้ำ")]
    public GameObject indicatorPrefab;    
    public GameObject tentaclePrefab;     
    
    [Space]
    public int tentacleCount = 2;         
    public float minAttackInterval = 5f;  
    public float maxAttackInterval = 10f; 
    public float indicatorDuration = 1.5f; 
    public float tentacleLingerTime = 1f; 
    
    [Space]
    public float aheadDistance = 3f;      
    public float attackRandomnessRadius = 2f; 

    private Transform boatTransform;
    private Rigidbody2D rb;

    // --- ตัวแปรใหม่สำหรับเก็บทิศทางการวิ่งของเรือ ---
    private Vector2 previousBoatPosition;
    private Vector2 boatDirection = Vector2.up; // เริ่มต้นให้จำว่าหันขึ้นบน

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        GameObject boatObj = GameObject.FindGameObjectWithTag("Player");
        if (boatObj != null) 
        {
            boatTransform = boatObj.transform;
            previousBoatPosition = boatTransform.position; // บันทึกตำแหน่งตอนเริ่ม
            StartCoroutine(AttackLoopRoutine());
        }
    }

    void Update()
    {
        if (boatTransform != null)
        {
            // -----------------------------------------------------
            // 1. คำนวณหาทิศทางที่เรือกำลังวิ่งไปจริงๆ
            // -----------------------------------------------------
            Vector2 currentPos = boatTransform.position;
            Vector2 moveDelta = currentPos - previousBoatPosition;
            
            // ถ้าเรือมีการขยับตัว (ขยับมากกว่า 0 นิดนึงกันบั๊ก)
            if (moveDelta.sqrMagnitude > 0.001f) 
            {
                boatDirection = moveDelta.normalized; // จำทิศทางล่าสุดไว้
            }
            previousBoatPosition = currentPos; // อัปเดตตำแหน่งไว้ใช้เทียบในเฟรมถัดไป

            // -----------------------------------------------------
            // 2. ระบบเสียงเข้าใกล้
            // -----------------------------------------------------
            if (isChasing)
            {
                float distanceToPlayer = Vector2.Distance(transform.position, currentPos);

                if (distanceToPlayer <= soundTriggerDistance && !hasPlayedSound)
                {
                    if (approachSound != null)
                    {
                        audioSource.PlayOneShot(approachSound); 
                        hasPlayedSound = true; 
                    }
                }
                else if (distanceToPlayer > soundTriggerDistance)
                {
                    hasPlayedSound = false;
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (!isChasing || boatTransform == null) return;

        Vector2 newPos = Vector2.MoveTowards(rb.position, (Vector2)boatTransform.position, chaseSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
    }

    IEnumerator AttackLoopRoutine()
    {
        while (isChasing && boatTransform != null)
        {
            float waitTime = Random.Range(minAttackInterval, maxAttackInterval);
            yield return new WaitForSeconds(waitTime);

            if (boatTransform == null || !isChasing) yield break;

            StartCoroutine(PerformTentacleAttack());
        }
    }

    IEnumerator PerformTentacleAttack()
    {
        Vector2 boatPos = (Vector2)boatTransform.position;

        // 🚨 แก้ไขตรงนี้: ใช้ boatDirection ที่เราคำนวณมา แทนการใช้ boatTransform.up
        Vector2 aheadPoint = boatPos + boatDirection * aheadDistance; 

        Vector2[] attackPositions = new Vector2[tentacleCount];
        GameObject[] indicators = new GameObject[tentacleCount];
        GameObject[] tentacles = new GameObject[tentacleCount];

        // วนลูปสร้างเงา
        for (int i = 0; i < tentacleCount; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * attackRandomnessRadius;
            attackPositions[i] = aheadPoint + randomOffset;
            indicators[i] = Instantiate(indicatorPrefab, attackPositions[i], Quaternion.identity);
        }

        yield return new WaitForSeconds(indicatorDuration);

        // ลบเงา
        for (int i = 0; i < tentacleCount; i++)
        {
            if (indicators[i] != null) Destroy(indicators[i]);
        }

        if (boatTransform == null || !isChasing) yield break;

        // วนลูปสร้างหนวด
        for (int i = 0; i < tentacleCount; i++)
        {
            tentacles[i] = Instantiate(tentaclePrefab, attackPositions[i], Quaternion.identity);
        }

        yield return new WaitForSeconds(tentacleLingerTime);

        // สั่งมุดน้ำและลบทิ้ง
        for (int i = 0; i < tentacleCount; i++)
        {
            if (tentacles[i] != null)
            {
                Animator tentacleAnim = tentacles[i].GetComponent<Animator>();
                if (tentacleAnim != null) 
                {
                    tentacleAnim.SetTrigger("Descend"); 
                }
                Destroy(tentacles[i], 1.5f); 
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Sand"))
        {
            isChasing = false;
            rb.linearVelocity = Vector2.zero;
            StopAllCoroutines(); 
            Debug.Log("หมึกเกยตื้น หยุดไล่ล่าและหยุดโจมตี!");
        }
        else if (other.CompareTag("Player"))
        {
            Debug.Log("💀 โดนหัวปลาหมึกชนตรงๆ! เด้งไปหน้า Game Over!");
            SceneManager.LoadScene(retrySceneName);
        }
    }
}