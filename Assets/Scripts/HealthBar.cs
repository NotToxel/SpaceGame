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
    private float lerpSpeed = 0.005f;

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

        // Testing
        if(Input.GetKeyDown(KeyCode.T)) {
            TakeDamage(10);
        }
    }

    
    void TakeDamage(float damage) {
        health -= damage;
        if(health < 0) {
            health = 0;
        }

       // healthSlider.value = health;
    }
}
