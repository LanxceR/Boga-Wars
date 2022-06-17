using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
    private LayerMask defaultLayerMask;

    [Header("Sprite Settings")]
    [SerializeField] private SpriteRenderer[] spriteRenderers;

    [Header("Health")]
    public float maxHealth;
    public float currentHealth;
    [SerializeField] private Color deadColor;

    [Header("States")]
    [SerializeField] private bool isInvincible = false;
    public bool isDead = false;
    public bool isInvulnerable = false;

    [Header("I-Frames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private Color blinkColor;
    private Color defaultColor;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask invulnerableLayerMask;
    [SerializeField] private LayerMask corpseLayerMask;

    [Header("Events")]
    public UnityEvent OnHit;
    public UnityEvent OnHealthReachZero;

    [Header("Audio")]
    [SerializeField] private AudioSource hitSfx;

    [Header("Misc")]
    public GameObject lastHitBy;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        defaultColor = spriteRenderers[0].color;
        defaultLayerMask = 1 << gameObject.layer;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
    }

    // This function is called when the object becomes enabled and active
    private void OnEnable()
    {
        Reset();
    }

    // Take damage
    public void TakeDamage(GameObject attacker, float damage)
    {
        // Set last hit by attacker
        lastHitBy = attacker;

        // If invincible or dead, return
        if (isInvulnerable || isDead) return;

        // Deduct current health by damage, but not exceeding 0
        currentHealth = currentHealth - damage < 0 ? 0 : currentHealth - damage;

        if (hitSfx)
        {
            // Play sfx
            hitSfx.Play();
        }
        else
        {
            if (transform.root.CompareTag("Player"))
            {
                // Play sfx
                AudioManager.GetInstance().PlayPlayerHit();
            }
            else
            {
                // Play sfx
                AudioManager.GetInstance().PlayEnemyHit();
            }
        }

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
            // Dies
            isDead = true;
            OnHealthReachZero?.Invoke();

            if (gameObject.tag == "Player")
            {
                // If player dies, game over
                GameManager.GetInstance().GameOver(lastHitBy);
                // Restart
                GameSceneManager.GetInstance().GotoSceneWithDelay(SceneName.STAGE_ONE, 5f);
            }
        }

        Debug.Log($"{gameObject.name} took {damage} damage from {lastHitBy.name}");
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

    // Perform various update to the object based on different states
    private void UpdateState()
    {
        if (isDead)
        {
            SetSpriteColor(deadColor);
            gameObject.layer = ToLayer(corpseLayerMask.value);
        }
        else if (isInvulnerable)
        {
            gameObject.layer = ToLayer(invulnerableLayerMask.value);
        }
        else
        {
            gameObject.layer = ToLayer(defaultLayerMask.value);
        }
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

    // Converts given bitmask to layer number
    public static int ToLayer(int bitmask)
    {
        int result = bitmask > 0 ? 0 : 31;
        while (bitmask > 1)
        {
            bitmask = bitmask >> 1;
            result++;
        }
        return result;
    }
}
