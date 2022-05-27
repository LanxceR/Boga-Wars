using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
    [Header("Sprite Settings")]
    [SerializeField] private SpriteRenderer[] spriteRenderers;

    [Header("Health")]
    [SerializeField] private float maxHealth;
    public float currentHealth;

    [Header("States")]
    [SerializeField] private bool isInvincible = false;
    public bool isDead = false;
    public bool isInvulnerable = false;

    [Header("I-Frames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private Color blinkColor;
    private Color defaultColor;

    [Header("Events")]
    public UnityEvent OnHit;
    public UnityEvent OnHealthReachZero;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        currentHealth = maxHealth;
        defaultColor = spriteRenderers[0].color;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // This function is called when the object becomes enabled and active
    private void OnEnable()
    {
        Reset();
    }

    // Take damage
    public void TakeDamage(GameObject attacker, float damage)
    {
        // If invincible or dead, return
        if (isInvulnerable || isDead) return;

        // Deduct current health by damage, but not exceeding 0
        currentHealth = currentHealth - damage < 0 ? 0 : currentHealth - damage;

        if (currentHealth > 0)
        {
            OnHit?.Invoke();

            if (iFramesDuration > 0)
            {
                // Start I-frames
                StartCoroutine(Invulnerability());
                // I-frames blink
                StartCoroutine(IFrames());
            }
        }
        else if (!isDead) // Prevent entity from dying again after its already dead
        {
            isDead = true;
            OnHealthReachZero?.Invoke();
        }

        Debug.Log($"{gameObject.name} took {damage} damage from {attacker.name}");
    }

    public void AddHealth(float value)
    {
        // Increase health but not past max health
        currentHealth = currentHealth + value > maxHealth ? maxHealth : currentHealth + value;
    }

    public void Reset()
    {
        // Reset health back to max health
        currentHealth = maxHealth;
        isDead = false;
    }

    public bool IsOnMaxHealth()
    {
        // Check if the player have max health or not
        return currentHealth == maxHealth;
    }

    private void SetSpriteColor(Color color)
    {
        foreach (SpriteRenderer sprite in spriteRenderers)
        {
            sprite.color = color;
        }
    }

    private IEnumerator Invulnerability()
    {
        isInvulnerable = true;

        yield return new WaitForSeconds(iFramesDuration);

        isInvulnerable = false;
    }

    private IEnumerator IFrames()
    {
        // I-frames
        while (isInvulnerable)
        {
            SetSpriteColor(defaultColor);
            yield return new WaitForSeconds(0.1f);
            SetSpriteColor(blinkColor);
            yield return new WaitForSeconds(0.1f);
        }

        SetSpriteColor(defaultColor);
    }
}
