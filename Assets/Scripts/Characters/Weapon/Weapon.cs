using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Weapon : MonoBehaviour
{
    private WeaponAnimation weaponAnim;
    private float cooldown = 0f;

    [Header("Main Settings")]
    [SerializeField] private float fireRateDelay = 0.1f;
    [SerializeField] private float damage = 1f;
    [SerializeField] private float range = 5f;
    [SerializeField] private float velocity = 5f;
    [SerializeField] private float startingAmmo = Mathf.Infinity;
    [SerializeField] private PoolObjectType projectileType; //Type of projectile to fire
    public float Ammo { get; private set; }

    [Header("Muzzles / Fire Positions")]
    [SerializeField] private List<WeaponMuzzle> muzzles;

    [Header("Misc Settings")]
    [SerializeField] private bool drawGizmo = false;

    // Start is called before the first frame update
    void Start()
    {
        // Get all muzzle children in this weapon object
        muzzles = GetComponentsInChildren<WeaponMuzzle>().ToList();

        // Check if this weapon have an animation or not
        weaponAnim = GetComponentInChildren<WeaponAnimation>();

        // Initialize ammo
        Ammo = startingAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        // Countdown cooldown until zero
        cooldown = cooldown - Time.deltaTime > 0 ? cooldown - Time.deltaTime : 0f;
    }

    // This function is called when the object becomes enabled and active
    private void OnEnable()
    {
        Reset();
    }

    // Reset weapon state (ammo, etc)
    public void Reset()
    {
        Ammo = startingAmmo;
    }

    // OnFire listener from InputAction "PlayerInput.inputaction"
    void OnFire()
    {
        Attack();
    }

    // Weapon attack
    public void Attack()
    {
        // If this weapon does NOT have an animation, fire/attack away
        // Otherwise call firing/attack in WeaponAnimation & animator
        if (!weaponAnim)
            ShootProjectile();
        else
            AttackAnim();
    }

    // Execute shoot anim (if available)
    public void AttackAnim()
    {
        if (cooldown <= 0f && Ammo > 0)
        {
            weaponAnim.AttackAnim();
        }
    }

    // Attempt to shoot projectile
    public void ShootProjectile()
    {
        if (cooldown <= 0f && Ammo > 0)
        {
            foreach (WeaponMuzzle muzzle in muzzles)
            {
                muzzle.SpawnProjectile(projectileType, range, damage, velocity);
            }

            //Set Cooldown
            cooldown = fireRateDelay;
        }
    }

    // Implement this OnDrawGizmosSelected if you want to draw gizmos only if the object is selected
    private void OnDrawGizmos()
    {
        if (!drawGizmo) return;

        // Weapon range
        Gizmos.color = new Color(240f / 255, 120f / 255, 46f / 255);
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
