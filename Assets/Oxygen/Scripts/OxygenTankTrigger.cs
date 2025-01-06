using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenTankTrigger : MonoBehaviour
{
    [SerializeField] private float oxygenTank = 10f;
    [SerializeField] private float wait = 20f;
    public GameObject oxygen;
    public OxygenManager oxygenManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) //Checks if player has collided with tank
        {
            oxygenManager.AddOxygen(oxygenTank); //Increases player's oxygen
            oxygen.SetActive(false); // Deactivates the oxygen tank
            StartCoroutine(ResetOxygen()); // Starts the coroutine to reset the tank
        }
    }

    private IEnumerator ResetOxygen()
    {
        yield return new WaitForSeconds(wait); // Waits for the specified time
        oxygen.SetActive(true); // Reactivates the oxygen tank
    }
}