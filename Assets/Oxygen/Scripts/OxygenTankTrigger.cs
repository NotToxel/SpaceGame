using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenTankTrigger : MonoBehaviour
{
    [SerializeField] private float oxygenTank = 10f;
    [SerializeField] private float wait = 100f;
    public GameObject oxygen;
    public OxygenManager oxygenManager;
    public QuestCatalyst questCatalyst;
    public QuestCatalyst questCatalyst1;
    public GameObject dialogueTrigger;
    public AudioSource oxygenSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) //Checks if player has collided with tank
        {
            oxygenManager.AddOxygen(oxygenTank); //Increases player's oxygen
            oxygen.SetActive(false); // Deactivates the oxygen tank
            if (MainManager.mainManager.questNames.Contains("Pick Up Oxygen."))
            {
                MainManager.mainManager.discoveryNames.Add("If you are low in oxygen, look for the flowers with a bubble on top. They will give you some oxygen!");
                oxygenSound.Play();
                questCatalyst.CompleteQuest();
                waitInBetween();
                questCatalyst1.CreateQuest();
                dialogueTrigger.SetActive(true);
            }
            StartCoroutine(ResetOxygen()); // Starts the coroutine to reset the tank
        }
    }

    private IEnumerator ResetOxygen()
    {
        yield return new WaitForSeconds(wait); // Waits for the specified time
        oxygen.SetActive(true); // Reactivates the oxygen tank
    }

    private IEnumerator waitInBetween()
    {
        yield return new WaitForSeconds(10f);
    }
}