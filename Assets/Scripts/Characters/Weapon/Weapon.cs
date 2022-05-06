using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Weapon : MonoBehaviour
{
    private Animator anim;
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

    // Start is called before the first frame update
    void Start()
    {
        // Get all muzzle children in this weapon object
        muzzles = GetComponentsInChildren<WeaponMuzzle>().ToList();

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
        Shoot();
    }

    // Attemp to shoot projectile
    public void Shoot()
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
}
