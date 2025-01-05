using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetUp : MonoBehaviour
{
    [Header("Animation")]
    public Animator animator;
    [SerializeField] private float duration1;
    [SerializeField] private float duration2;
    [SerializeField] private float duration3;
    public GameObject dialogue;
    public GameObject player;
    public GameObject cam;
    public QuestCatalyst quest;
    
    void Start()
    {
        // player = GameObject.FindWithTag("Player");
        StartCoroutine(NextAnimation(duration1));
    }

    IEnumerator NextAnimation(float duration1)
    {
        yield return new WaitForSeconds(duration1);
        animator.SetBool("sitOnBed", true);
        yield return new WaitForSeconds(duration2);
        animator.SetBool("standUp", true);
        yield return new WaitForSeconds(duration3);
        player.SetActive(true);
        dialogue.SetActive(true);
        quest.CreateQuest();
        cam.SetActive(false);
    }

}
