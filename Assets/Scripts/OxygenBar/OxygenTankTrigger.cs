/*****************
This script checks if the player has collided with the oxygen tank.
If the player has collided, the player will gain more oxygen 
and the oxygen tank will be destroyed.
*******************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenTankTrigger : MonoBehaviour
{
    public float oxygenTank = 10f;
    public OxygenTrigger oxygenTrigger;
    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")){ //Checks if player has collided with tank
            oxygenTrigger.getOxygen(oxygenTank); //Increases player's oxygen
            Destroy(gameObject); //Destroys oxygen tank once player has used it
        }
    }
}
