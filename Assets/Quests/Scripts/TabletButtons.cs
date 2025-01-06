using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletButtons : MonoBehaviour
{
    [SerializeField] private GameObject questSection;
    [SerializeField] private GameObject discoverySection;
    [SerializeField] private GameObject missionSection;
    [SerializeField] private GameObject completedInTablet1;
    [SerializeField] private GameObject completedInTablet2;

    public void QuestButtonClicked()
    {
        missionSection.SetActive(false);
        discoverySection.SetActive(false);
        questSection.SetActive(true);
    }

    public void DiscoveryButtonClicked()
    {
        missionSection.SetActive(false);
        questSection.SetActive(false);
        discoverySection.SetActive(true);
    }

    public void MissionButtonClicked()
    {
        completedInTablet1.SetActive(false);
        completedInTablet2.SetActive(false);
        missionSection.SetActive(true);
        questSection.SetActive(false);
        discoverySection.SetActive(false);
    }
}
