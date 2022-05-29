using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class KnockbackSystem : MonoBehaviour
{
    private Rigidbody2D rb;
    private Moveable moveable;

    [Header("Main Settings")]
    [SerializeField] private PhysicsMaterial2D physMaterial;
    [SerializeField] private bool knockbackImmune = false;
    [SerializeField] private float knockbackResist = 1f;
    [SerializeField] private float recoveryTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveable = GetComponent<Moveable>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This function is called every fixed framerate frame, if the MonoBehaviour is enabled
    private void FixedUpdate()
    {

    }

    public void DoKnockback(float force, Vector2 direction, bool canResist, bool canRecover)
    {
        // If knockback immune, return
        if (knockbackImmune) return;

        // Temporarily disable movement (simulate stagger)
        moveable.enabled = false;

        float finalForce;
        if (canResist)
        {
            // Calculate force with resistance
            finalForce = force - knockbackResist > 0 ? force - knockbackResist : 0;
        }
        else
        {
            finalForce = force;
        }

        // Apply knockback
        rb.AddForce(direction.normalized * (finalForce), ForceMode2D.Impulse);

        if (canRecover)
        {
            // Recover from knockback after recoveryTime seconds has passed
            StartCoroutine(RecoverFromKnockback());
        }
        else
        {
            EnablePhysisMaterial();
        }

        Debug.Log($"{gameObject.name} took {force - moveable.GetDirection().magnitude} knockback");
    }

    private IEnumerator RecoverFromKnockback()
    {
        yield return new WaitForSeconds(recoveryTime);
        moveable.enabled = true;
    }

    private void EnablePhysisMaterial()
    {
        rb.sharedMaterial = physMaterial;
    }

    private void DisablePhysisMaterial()
    {
        rb.sharedMaterial = null;
    }
}
