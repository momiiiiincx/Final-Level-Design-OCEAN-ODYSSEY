using UnityEngine;
using TMPro;

public class CodeSystem : MonoBehaviour
{
    public GameObject codeUI;
    public TMP_InputField inputField;
    public TextMeshProUGUI resultText;
    public string correctCode = "1234";
    public GameObject redCircle;
    public RedCircleGoal redCircleGoal;

    private bool isOpen = false;

    void Start()
    {
        codeUI.SetActive(false);
        Time.timeScale = 1f;
    }

    void Update()
    {
        // 🔥 กด ESC ออกได้เสมอ
        if (isOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseUI();
        }
    }

    public void OpenUI()
    {
        codeUI.SetActive(true);
        Time.timeScale = 0f;
        isOpen = true;

        // 🔥 รีเซ็ตทุกครั้งที่เปิด
        inputField.text = "";
        resultText.text = "";
        inputField.ActivateInputField();
    }

    public void CloseUI()
    {
        codeUI.SetActive(false);
        Time.timeScale = 1f;
        isOpen = false;

        inputField.DeactivateInputField();
    }

    public void CheckCode()
    {
        Debug.Log("CheckCode called"); // 🔥 เอาไว้เช็ค

        if (inputField.text == correctCode)
        {
            resultText.text = "Correct!";
            redCircle.SetActive(true);
            redCircleGoal.Unlock(); 
            CloseUI();
        }
        else
        {
            resultText.text = "Wrong!";
            inputField.text = "";
        }
    }
}