/********
This script checks if the player has left the ship and changes the player's
oxygen level according to where they are. 
If the player has left the ship, their oxygen becomes limited and
decreases until 0.
If the player enters the ship again, they will have an unlimited supply of oxygen,
and so their oxygen levels should increase gradually.
*********/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OxygenTrigger : MonoBehaviour
{
    [SerializeField] GameObject amount;
    public HealthBar healthBar;
    public Slider oxygenSlider;
    public float maxO = 100f;
    public float oxygenLvl;
    public float healthLvl;
    public float oxygenTickRate = 5f;
    public float breatheTickRate = 1f;
    public float healthTickRate = 1f;
    public float damageNoAir = 1f;
    public bool oxygenPresent = true;

    void Start()
    {
        oxygenLvl = maxO;

        //Finds the health bar if it's not been assigned so it actuallly works
        if (healthBar == null) {
            healthBar = FindFirstObjectByType<HealthBar>();
        }
        
        UpdateAmount();
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
        Debug.Log("Routine Started!");
        // Oxygen ticks down while player is in an area lacking oxygen (i.e. out of ship)
        while(oxygenPresent == false && oxygenLvl > 0) {
            yield return new WaitForSeconds(oxygenTickRate);
            OxygenDepletion(1); // Currently: 1 oxygen per second
            UpdateAmount();
        }

        // When player enters ship, their oxygen increases by 1f per second
        while(oxygenPresent == true && oxygenLvl != maxO){
            yield return new WaitForSeconds(breatheTickRate);
            Breathe(1);
            UpdateAmount();
        }

        while(oxygenPresent == false && oxygenLvl == 0){
            yield return new WaitForSeconds(healthTickRate);
            healthBar.TakeDamage(damageNoAir);
        }
    }

    void UpdateAmount() {
        TextMeshProUGUI text = amount.GetComponent<TextMeshProUGUI>();
        int oxygenPercentage = (int)Math.Round((oxygenLvl / maxO) * 100);
        text.SetText(oxygenPercentage + "%");
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

        if(healthBar.health != maxO){
            healthBar.regenHP(1f);
        }
    }

    

    public void getOxygen(float oxygenTank){
        oxygenLvl += oxygenTank;
        if (oxygenLvl > maxO){ //Oxygen is never above max
            oxygenLvl = maxO;
        }
        oxygenSlider.value = oxygenLvl;
    }
}