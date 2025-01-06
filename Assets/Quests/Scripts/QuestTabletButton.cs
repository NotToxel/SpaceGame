using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using TMPro;

public class QuestTabletButton : MonoBehaviour
{
    [SerializeField] private GameObject questPage;
    [SerializeField] private  TMP_Text questTextBox;
    [SerializeField] private  TMP_Text discoveryTextBox;
    [SerializeField] private GameObject notification;
    [SerializeField] private GameObject completed;
    [SerializeField] private string[] noQuestsText;
    [SerializeField] private string[] noDiscoveriesText;

    public AudioSource openTabletClip;
    public AudioSource closeTabletClip;

    private bool openTablet;  

    public void OpenQuestTablet()
    {
        openTablet = !openTablet;
        CreatePage();
        WriteQuests();
        WriteDiscovery();
    }

    private void CreatePage()
    {
        if (questPage != null && notification != null && completed != null)
        {
            if (openTablet)
            {
                openTabletClip.Play();
                questPage.SetActive(true);
                notification.SetActive(false);
                completed.SetActive(false);
            }
            else{
                openTabletClip.Play();
                questPage.SetActive(false);
            }
        }
    }

    private void WriteQuests()
    {
        if (questTextBox != null) 
        {
            if (MainManager.mainManager.questNames.Count == 0)
            {
                if (noQuestsText != null)
                {
                    int randomNum = (Random.Range(0, noQuestsText.Length));
                    questTextBox.text = noQuestsText[randomNum];
                }
            }

            else
            {
                StringBuilder stringBuilder = new();
                foreach (string quest in MainManager.mainManager.questNames)
                {
                    stringBuilder.AppendLine(quest);
                }
                questTextBox.text = stringBuilder.ToString();
            }

            questTextBox.rectTransform.sizeDelta = new Vector2(questTextBox.rectTransform.sizeDelta.x, questTextBox.preferredHeight);

        }
    }

    private void WriteDiscovery()
    {
        if (discoveryTextBox != null) 
        {
            if (MainManager.mainManager.discoveryNames.Count == 0)
            {
                if (noDiscoveriesText != null)
                {
                    int randomNum = (Random.Range(0, noDiscoveriesText.Length));
                    discoveryTextBox.text = noDiscoveriesText[randomNum];
                }
            }

            else
                {
                    StringBuilder stringBuilder = new();
                    foreach (string discovery in MainManager.mainManager.discoveryNames)
                    {
                        stringBuilder.AppendLine(discovery);
                    }
                    discoveryTextBox.text = stringBuilder.ToString();
                }

                discoveryTextBox.rectTransform.sizeDelta = new Vector2(questTextBox.rectTransform.sizeDelta.x, questTextBox.preferredHeight);
        }
    }
}
