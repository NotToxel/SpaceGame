using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Slider easeHealthSlider;
    public float maxHP = 100f;
    public float health;
    public float naturalRegenRate = 5f;
    private float lerpSpeed = 0.025f;
    private float combatTimer = 5f;
    private float combatCD = 5f;
    private bool isInCombat = false;

    // Start is called before the first frame update
    void Start() {
        health = maxHP;
    }

    // Update is called once per frame
    void Update() {
        // Update Health Slider
        if(healthSlider.value != health) {
            healthSlider.value = health;
        }

        // Update Ease Health Slider
        if(healthSlider.value != easeHealthSlider.value) {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, health, lerpSpeed);
        }

        // Exit combat state if Player has not taken any damage in 10s
        if (isInCombat == true) {
            combatTimer -= Time.deltaTime;
            if(combatTimer <= 0) {
                ExitCombat();
            }
        }

        // Regenerate Health while Player is out of Combat
        if (isInCombat == false) {
            regenHP(naturalRegenRate);
        }


        // Testing
        if(Input.GetKeyDown(KeyCode.Space)) {
            TakeDamage(10);
        }
    }

    private void regenHP(float rate) {
        health += rate * Time.deltaTime;
        health = Mathf.Clamp(health, 0, maxHP);
    }

    public void EnterCombat() {
        isInCombat = true;
        combatTimer = combatCD;
    }

    public void ExitCombat() {
        isInCombat = false;
    }

    
    void TakeDamage(float damage) {
        EnterCombat();
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHP);
    }
}

