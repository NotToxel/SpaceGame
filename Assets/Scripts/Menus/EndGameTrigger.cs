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
    //loads credits scene upon trigger
    private void EndGame() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadSceneAsync(2);
    }
}
