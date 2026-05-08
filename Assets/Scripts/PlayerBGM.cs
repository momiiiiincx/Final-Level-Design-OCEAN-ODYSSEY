using UnityEngine;

// บังคับให้ Unity สร้าง AudioSource ให้อัตโนมัติตอนลากสคริปต์ไปใส่
[RequireComponent(typeof(AudioSource))]
public class PlayerBGM : MonoBehaviour
{
    [Header("ตั้งค่าเพลงพื้นหลัง (BGM)")]
    public AudioClip bgmClip; // ช่องสำหรับลากไฟล์เพลงมาใส่

    [Range(0f, 1f)]
    public float bgmVolume = 0.5f; // ปรับความดังของเพลงได้ตั้งแต่ 0 ถึง 1

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // เช็คว่ามีการใส่ไฟล์เพลงมาแล้วหรือยัง
        if (bgmClip != null)
        {
            audioSource.clip = bgmClip;
            audioSource.volume = bgmVolume;
            audioSource.loop = true; // ตั้งค่าให้เพลงเล่นวนซ้ำเรื่อยๆ
            audioSource.playOnAwake = false; // ปิด Play On Awake ไว้ก่อน เพื่อให้โค้ดสั่ง Play เอง
            
            audioSource.Play(); // เริ่มเล่นเพลง
        }
    }

    // ฟังก์ชันนี้เผื่อเรียกใช้ตอน Game Over หรือเปลี่ยน Scene เพื่อหยุดเพลง
    public void StopBGM()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}