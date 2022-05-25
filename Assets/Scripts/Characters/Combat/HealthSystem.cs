using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
    public float CurrentHealth { get; private set; }
    private bool isInvulnerable;

    [Header("Health")]
    [SerializeField] private float maxHealth;
    [SerializeField] private bool isInvincible = false;
    [SerializeField] private Behaviour[] components;
    public UnityEvent OnHit;
    public UnityEvent OnHealthReachZero;
    public bool isDead = false;

    [Header("I-Frames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    [SerializeField] private Color flashColor;
    private Color defaultColor;

    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Take damage
    public void TakeDamage(GameObject attacker, float damage)
    {
        // If invincible, return
        if (isInvincible) return;

        Debug.Log($"{gameObject.name} took {damage} damage from {attacker.name}");
    }
}
