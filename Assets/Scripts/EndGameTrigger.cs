using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameTrigger : MonoBehaviour
{
    public string targetTag = "Pickup";

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag(targetTag)) {
            EndGame();
        }
    }

    private void EndGame() {
        SceneManager.LoadSceneAsync(2);
    }
}
