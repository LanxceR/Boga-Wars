using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAnimation : MonoBehaviour
{
    private Animator anim;
    private Projectile projectile;

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
    }
    // Hit an enemy
    public void TriggerHitEnemy()
    {
        anim.SetTrigger("Hit Enemy");
    }
    // Hit nothing and despawn (because of out of range)
    public void TriggerOut()
    {
        anim.SetTrigger("Out");
    }
}
