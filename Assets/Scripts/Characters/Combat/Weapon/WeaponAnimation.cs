using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class WeaponAnimation : MonoBehaviour
{
    private Animator anim;
    private Weapon weapon;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        weapon = GetComponentInParent<Weapon>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Trigger shoot anim
    public void AttackAnim()
    {
        anim.SetTrigger("Attack");
    }

    // Execute attack in Weapon.cs, call this in animator events
    public void ExecuteAttackWeapon()
    {
        weapon.ShootProjectile();
    }
}
