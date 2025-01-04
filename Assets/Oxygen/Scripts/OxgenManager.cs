using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OxygenManager : MonoBehaviour
{
    public static OxygenManager Instance { get; private set; } // Singleton instance

    [SerializeField] private GameObject amount; // UI for oxygen percentage
    public HealthBar healthBar;                 // Player's health bar
    public Slider oxygenSlider;                 // Oxygen slider UI

    public float maxOxygen = 100f;
    public float oxygenLevel;
    public float oxygenTickRate = 5f;
    public float breatheTickRate = 1f;
    public float healthTickRate = 1f;
    public float damageNoAir = 1f;

    private bool oxygenPresent = true; // Global oxygen state
    private Coroutine oxygenRoutine;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        oxygenLevel = maxOxygen;

        // Find the health bar if not assigned
        if (healthBar == null)
        {
            healthBar = FindFirstObjectByType<HealthBar>();
        }

        UpdateOxygenUI();
    }

    public void SetOxygenState(bool isOxygenPresent)
    {
        if (oxygenPresent == isOxygenPresent)
            return;

        oxygenPresent = isOxygenPresent;

        if (oxygenRoutine != null)
        {
            StopCoroutine(oxygenRoutine);
        }

        if (oxygenPresent)
        {
            oxygenRoutine = StartCoroutine(OxygenRegenerationRoutine());
        }
        else
        {
            oxygenRoutine = StartCoroutine(OxygenDepletionRoutine());
        }
    }

    private IEnumerator OxygenDepletionRoutine()
    {
        while (!oxygenPresent && oxygenLevel > 0)
        {
            yield return new WaitForSeconds(oxygenTickRate);
            DecreaseOxygen(1);
        }

        while (!oxygenPresent && oxygenLevel == 0)
        {
            yield return new WaitForSeconds(healthTickRate);
            healthBar.TakeDamage(damageNoAir);
        }
    }

    private IEnumerator OxygenRegenerationRoutine()
    {
        while (oxygenPresent && oxygenLevel < maxOxygen)
        {
            yield return new WaitForSeconds(breatheTickRate);
            IncreaseOxygen(1);
        }
    }

    private void DecreaseOxygen(float amount)
    {
        oxygenLevel -= amount;
        if (oxygenLevel < 0)
        {
            oxygenLevel = 0;
        }
        UpdateOxygenUI();
    }

    private void IncreaseOxygen(float amount)
    {
        oxygenLevel += amount;
        if (oxygenLevel > maxOxygen)
        {
            oxygenLevel = maxOxygen;
        }
        UpdateOxygenUI();
        healthBar.regenHP(1f); // Optionally regenerate health
    }

    private void UpdateOxygenUI()
    {
        if (amount != null)
        {
            TextMeshProUGUI text = amount.GetComponent<TextMeshProUGUI>();
            int oxygenPercentage = Mathf.RoundToInt((oxygenLevel / maxOxygen) * 100);
            text.SetText($"{oxygenPercentage}%");
        }

        if (oxygenSlider != null)
        {
            oxygenSlider.value = oxygenLevel;
        }
    }

    public void AddOxygen(float oxygenTank)
    {
        IncreaseOxygen(oxygenTank);
    }
}
