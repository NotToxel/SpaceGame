using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletButtons : MonoBehaviour
{
    [SerializeField] private GameObject questSection;
    [SerializeField] private GameObject discoverySection;
    [SerializeField] private GameObject missionSection;
    public GameObject completedInTablet;

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
        completedInTablet.SetActive(false);
        missionSection.SetActive(true);
        questSection.SetActive(false);
        discoverySection.SetActive(false);
    }
}
