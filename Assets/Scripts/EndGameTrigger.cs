using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameTrigger : MonoBehaviour
{
    public string targetTag = "Pickup";

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag(targetTag)) {
            EndGame();
        }
    }

    private void EndGame() {
        Debug.Log("Game Over!");
    }
}
