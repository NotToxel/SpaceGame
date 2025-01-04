using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth;
    [HideInInspector]
    public float currentHealth;
    AiAgent agent;
    SkinnedMeshRenderer skinnedMeshRenderer;
    UIHealthBar healthBar;
    public float blinkIntensity;
    public float blinkDuration;
    float blinkTimer;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<AiAgent>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        healthBar = GetComponentInChildren<UIHealthBar>();
        currentHealth = maxHealth;
    }

  public void TakeDamage(float amount, Vector3 direction)
    {
        currentHealth -= amount;
        healthBar.SetHealthBarPercentage(currentHealth / maxHealth);
        if (currentHealth <= 0.0f)
        {
            Die(direction);
        }
        blinkTimer = blinkDuration;
    }

    private void Die(Vector3 direction)
    {
        AiDeathState deathState = agent.stateMachine.GetState(AiStateId.Death) as AiDeathState;
        deathState.direction = direction;
        agent.stateMachine.ChangeState(AiStateId.Death);
        healthBar.gameObject.SetActive(false);
    }

    private void Update()
    {
        blinkTimer -= Time.deltaTime;
        float lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
        float intensity = (lerp * blinkIntensity) + 1.0f;
        skinnedMeshRenderer.material.color = Color.white * intensity;
    }
}
