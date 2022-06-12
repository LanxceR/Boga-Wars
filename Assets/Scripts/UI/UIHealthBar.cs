using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private Image currentHealthBar;
    [SerializeField] private Image maxHealthBar;

    private HealthSystem playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        // Fetch playerHealth
        playerHealth = GameManager.GetInstance().ActivePlayer.GetComponent<HealthSystem>();

        // Call init method
        InitializeHealthbar();
    }

    // Update is called once per frame
    void Update()
    {
        // Call healthbar update method
        UpdateHealthbar();
    }

    private void InitializeHealthbar()
    {
        // Initialize max healthbar
        if (playerHealth)
        {
            maxHealthBar.fillAmount = playerHealth.currentHealth / 20;
            Debug.Log("Max Healthbar UI initialized");
        }
    }

    private void UpdateHealthbar()
    {
        // Initialize max healthbar
        if (playerHealth)
        {
            // Update current health bar
            currentHealthBar.fillAmount = playerHealth.currentHealth / 20;
        }
        else
        {
            // If there's no active player, set healthbar to zero
            currentHealthBar.fillAmount = 0;
        }
    }
}
