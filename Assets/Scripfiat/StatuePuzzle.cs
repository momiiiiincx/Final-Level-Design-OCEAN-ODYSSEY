using UnityEngine;
using System.Collections.Generic;

public class StatuePuzzle : MonoBehaviour
{
    public int[] correctOrder = { 1, 3, 2, 6, 4, 5 };
    private int currentIndex = 0;

    public GameObject noteObject;

    private List<Statue> pressedStatues = new List<Statue>();
    private Statue wrongStatue; 
    public ObjectiveManager objectiveManager;// 🔥 เก็บตัวที่ผิด

    public void PressStatue(Statue statue)
    {
        int id = statue.statueID;

        if (id == correctOrder[currentIndex])
        {
            statue.SetGreen();
            pressedStatues.Add(statue);

            currentIndex++;

            if (currentIndex >= correctOrder.Length)
            {
               
                CompletePuzzle();
            }
        }
        else
        {
            statue.SetRed();
            wrongStatue = statue; // 🔥 จำไว้

            Invoke("ResetPuzzle", 1f);
        }
    }

    void CompletePuzzle()
    {
        noteObject.SetActive(true);
        objectiveManager.SetObjective("Find the note");
    }

    void ResetPuzzle()
    {
        currentIndex = 0;

        // 🔄 รีเซ็ตตัวที่กดถูก
        foreach (Statue s in pressedStatues)
        {
            s.ResetColor();
        }

        pressedStatues.Clear();

        // 🔄 รีเซ็ตตัวที่กดผิด
        if (wrongStatue != null)
        {
            wrongStatue.ResetColor();
            wrongStatue = null;
        }
    }
}