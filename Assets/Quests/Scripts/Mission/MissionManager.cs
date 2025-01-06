using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MissionManager : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI rewardsText;
    public Queue<string> titles;
    public Queue<string> rewards;
    public PlayerController playerController;

    void Start()
    {
        titles = new Queue<string>();
        rewards = new Queue<string>();
    }

    public void NextMission(Mission mission)
    {
        titles.Clear();

        foreach (string title in mission.titles)
        {
            titles.Enqueue(title);
        }

        DisplayNextMission();
    }

    public void DisplayNextMission()
    {
        if (titles.Count == 0)
        {
            EndMissions();
            return;
        }

        string title = titles.Dequeue();
        titleText.text = title;
    }

    public void EndMissions()
    {
        Debug.Log("All missions have been completed");
    }
}
