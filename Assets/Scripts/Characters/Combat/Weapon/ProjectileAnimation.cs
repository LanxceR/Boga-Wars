using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileAnimation : MonoBehaviour
{
    private Animator anim;
    private Projectile projectile;

    [SerializeField] private UnityEvent onProjectileSpawn;
    [SerializeField] private UnityEvent onProjectileOut;
    [SerializeField] private UnityEvent onProjectileHit;
    [SerializeField] private UnityEvent onProjectileHitTarget;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        projectile = GetComponent<Projectile>();
    }

    // Update is called once per frame
    void Update()
    {
        if (projectile.IsOutOfRange() && !projectile.HasHit())
        {
            TriggerOut();
        }
    }

    // This function is called when the object becomes enabled and active
    private void OnEnable()
    {
        onProjectileSpawn?.Invoke();
    }


    // Hit a wall, obstacle, etc
    public void TriggerHit()
    {
        anim.SetTrigger("Hit");
        onProjectileHit?.Invoke();
    }
    // Hit a target
    public void TriggerHitTarget()
    {
        anim.SetTrigger("Hit Target");
        onProjectileHitTarget?.Invoke();
    }
    // Hit nothing and despawn (because of out of range)
    public void TriggerOut()
    {
        anim.SetTrigger("Out");
        onProjectileOut?.Invoke();
    }
}
