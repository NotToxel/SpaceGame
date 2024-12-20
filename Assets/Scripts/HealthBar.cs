using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public OxygenTrigger oxygenTrigger;
    public Slider healthSlider;
    public Slider easeHealthSlider;
    public float maxHP = 100f;
    public float health;
    public float naturalRegenRate = 1f;
    private float lerpSpeed = 0.025f;
    private float combatTimer = 5f;
    private float combatCD = 5f;
    private bool isInCombat = false;
    public float regenCooldown = 1f; // Regenerate health every 1 second
    private float regenTimer = 0f;   // Internal timer for regen

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
            if (combatTimer <= 0) {
                ExitCombat();
            }
        }

        // Regenerate Health while Player is out of Combat
        if (isInCombat == false) {
            regenTimer -= Time.deltaTime;
            if (regenTimer <= 0f) {
                regenHP(naturalRegenRate);
                regenTimer = regenCooldown; // Reset the regen timer
            }
        }
    }

    public void regenHP(float rate) {
        health += rate;
        health = Mathf.Clamp(health, 0, maxHP);
    }

    public void EnterCombat() {
        isInCombat = true;
        combatTimer = combatCD;
    }

    public void ExitCombat() {
        isInCombat = false;
    }
    
    public void TakeDamage(float damage) {
        EnterCombat();
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHP);
    }
}