using UnityEngine;
using UnityEngine.InputSystem;

// ใส่ที่ตัว Mechanic (ช่าง)
public class FindPersonTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            QuestManager qm = Object.FindFirstObjectByType<QuestManager>();
            if (qm != null && qm.questPhase == 2) qm.foundMechanic = true;
        }
    }
}


