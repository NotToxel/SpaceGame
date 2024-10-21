using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenBar : MonoBehaviour {

    public Slider oxygenSlider;
    public float maxO = 100f;
    public float oxygenLvl;
    public float oxygenTickRate = 1f;
    private bool oxygenPresent = false;


    // Start is called before the first frame update
    void Start()
    {
        oxygenLvl = maxO;
        StartCoroutine(OxygenDepletionRoutine());
    }

    IEnumerator OxygenDepletionRoutine() {
        // Oxygen ticks down while player is in an area lacking oxygen (i.e. out of ship)
        while(oxygenPresent == false && oxygenLvl > 0) {
            yield return new WaitForSeconds(oxygenTickRate);
            OxygenDepletion(1); // Currently: 1 oxygen per second
        }
    }

    void OxygenDepletion(float amount) {
        oxygenLvl -= amount;

        // Oxygen is never negative
        if(oxygenLvl < 0) {
            oxygenLvl = 0;
        }

        oxygenSlider.value = oxygenLvl;
    }
}
