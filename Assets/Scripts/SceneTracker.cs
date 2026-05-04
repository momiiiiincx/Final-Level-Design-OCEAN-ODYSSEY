using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneTracker
{
    // เปลี่ยนมาดักจับตั้งแต่ก่อนโหลดซีน
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // ป้องกันการทำงานซ้ำซ้อน
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 1. ตรวจสอบว่า "ต้องไม่ใช่" ซีน GameOver 
        // 2. ตรวจสอบว่า "ต้องไม่ใช่" หน้าเมนูหลัก/หน้าแรก (ถ้ามีหน้าเมนูหลักให้แก้ชื่อตรง Departure เป็นชื่อหน้านั้น)
        if (scene.name != "GameOver" && scene.name != "Departure") 
        {
            PlayerPrefs.SetString("LastLevel", scene.name);
            PlayerPrefs.Save(); // บังคับเซฟลงระบบทันที

            // บรรทัดนี้จะไปโผล่ใน Console เพื่อยืนยันว่าระบบจำด่านได้แล้ว
            Debug.Log("✅ [SceneTracker] บันทึกด่านล่าสุดแล้วคือ: " + scene.name); 
        }
    }
}