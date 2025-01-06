using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MissionBox : MonoBehaviour
{
    public string[] titlesList;

    [TextArea(3,10)]
    public string[] rewardsList;
    public Slider progressSlider;
    public int currentProgress;
    // public GameObject missionTrigger;
    public int timesComplete = 0;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI rewardsText;
    public Queue<string> titles;
    public Queue<string> rewards;
    public PlayerController playerController;
    public bool hasMissionEnded = false;
    public GameObject completedNotification;
    public GameObject completedInTablet1;
    public GameObject completedInTablet2;


    void Start()
    {
        titles = new Queue<string>();
        rewards = new Queue<string>();

        AddMissions();
    }

    void Update()
    {
        if (hasMissionEnded == false)
        {
            if (currentProgress == progressSlider.maxValue)
            {
                completedNotification.SetActive(true);
                completedInTablet1.SetActive(true);
                completedInTablet2.SetActive(true);
                DisplayNextMission();
                currentProgress = 0;
            }
            
            progressSlider.value = currentProgress;
        }
    }

    public void AddMissions()
    {
        titles.Clear();

        foreach (string title in titlesList)
        {
            titles.Enqueue(title);
        }

        rewards.Clear();

        foreach (string reward in rewardsList)
        {
            rewards.Enqueue(reward);
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

        string reward = rewards.Dequeue();
        rewardsText.text = reward;
    }

    public void EndMissions()
    {
        titleText.text = "You have finished all of these missions!";
        rewardsText.text = "You have finished all of these missions!";
        hasMissionEnded = true;
    }

    public void PressButton()
    {
        currentProgress += 1;
        Debug.Log(currentProgress);
    }
}
