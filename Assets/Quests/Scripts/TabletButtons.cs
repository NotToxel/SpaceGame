using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletButtons : MonoBehaviour
{
    [SerializeField] private GameObject questSection;
    [SerializeField] private GameObject discoverySection;


    public void QuestButtonClicked()
    {
        discoverySection.SetActive(false);
        questSection.SetActive(true);
    }

    public void DiscoveryButtonClicked()
    {
        questSection.SetActive(false);
        discoverySection.SetActive(true);
    }
}
