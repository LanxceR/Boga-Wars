using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMuzzle : MonoBehaviour
{
    // Fire a projectile from this muzzle/fire position
    public void SpawnProjectile(PoolObjectType poolObjectType, float range, float damage, float velocity, float knockbackForce)
    {
        PoolObject projectile = ObjectPooler.GetInstance().RequestObject(poolObjectType);
        projectile.Activate(transform.position, transform.rotation);
        projectile.GetComponent<Projectile>().SetDamage(damage);
        projectile.GetComponent<Projectile>().SetRange(range);
        projectile.GetComponent<Projectile>().SetVelocity(velocity);
    }
}
