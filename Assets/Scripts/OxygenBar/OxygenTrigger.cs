/********
This script checks if the player has left the ship and changes the player's
oxygen level according to where they are. 
If the player has left the ship, their oxygen becomes limited and
decreases until 0.
If the player enters the ship again, they will have an unlimited supply of oxygen,
and so their oxygen levels should increase gradually.
*********/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenTrigger : MonoBehaviour
{
    public Slider oxygenSlider;
    public float maxO = 100f;
    public float oxygenLvl;
    public float oxygenTickRate = 1f;
    public float breatheTickRate = 1f;
    private bool oxygenPresent = true;

    void Start()
    {
        oxygenLvl = maxO;
    }

    //Checks if player has collided with object (player has left ship)
    void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")){
            if (oxygenPresent == true){oxygenPresent = false;}
            else{oxygenPresent = true;}
            Debug.Log(oxygenPresent); //TESTING
            
            StartCoroutine(OxygenDepletionRoutine());
        }
    }

    IEnumerator OxygenDepletionRoutine() {
        // Oxygen ticks down while player is in an area lacking oxygen (i.e. out of ship)
        while(oxygenPresent == false && oxygenLvl > 0) {
            yield return new WaitForSeconds(oxygenTickRate);
            OxygenDepletion(1); // Currently: 1 oxygen per second
        }

        // When player enters ship, their oxygen increases by 1f per second
        while(oxygenPresent == true && oxygenLvl != maxO){
            yield return new WaitForSeconds(breatheTickRate);
            Breathe(1);
        }
    }

    void OxygenDepletion(float amount) {
        oxygenLvl -= amount;

        if(oxygenLvl < 0) { // Oxygen is never negative
            oxygenLvl = 0;
        }

        oxygenSlider.value = oxygenLvl;
    }

    void Breathe(float amount){
        oxygenLvl += amount;

        if (oxygenLvl > maxO){ //oxygen is never above max
            oxygenLvl = maxO;
        }
        oxygenSlider.value = oxygenLvl;
    }

    public void getOxygen(float oxygenTank){
        oxygenLvl += oxygenTank;
        if (oxygenLvl > maxO){ //Oxygen is never above max
            oxygenLvl = maxO;
        }
        oxygenSlider.value = oxygenLvl;
    }
}