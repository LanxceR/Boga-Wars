using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileAnimation : MonoBehaviour
{
    private Animator anim;
    private Projectile projectile;

    [SerializeField] private UnityEvent onProjectileOut;
    [SerializeField] private UnityEvent onProjectileHit;
    [SerializeField] private UnityEvent onProjectileHitEnemy;

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

    // Hit a wall, obstacle, etc
    public void TriggerHit()
    {
        anim.SetTrigger("Hit");
        onProjectileHit?.Invoke();
    }
    // Hit an enemy
    public void TriggerHitEnemy()
    {
        anim.SetTrigger("Hit Enemy");
        onProjectileHitEnemy?.Invoke();
    }
    // Hit nothing and despawn (because of out of range)
    public void TriggerOut()
    {
        anim.SetTrigger("Out");
        onProjectileOut?.Invoke();
    }
}
