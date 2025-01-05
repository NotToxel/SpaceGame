using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionTrigger : MonoBehaviour
{
    public Mission mission;
    
    void Start()
    {
        FindObjectOfType<MissionManager>().NextMission(mission);
    }
}
