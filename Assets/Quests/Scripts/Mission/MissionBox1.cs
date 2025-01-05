using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionBox1 : MonoBehaviour
{
    public Slider progressSlider;
    public int currentProgress;
    public GameObject missionTrigger;

    void Update()
    {
        if (currentProgress == progressSlider.value)
        {
            missionTrigger.SetActive(true);
        }
        
        progressSlider.value = currentProgress;
    }

    public void PressButton()
    {
        currentProgress += 1;
    }
}
