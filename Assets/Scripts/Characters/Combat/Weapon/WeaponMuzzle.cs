using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMuzzle : MonoBehaviour
{
    // Fire a projectile from this muzzle/fire position
    public void SpawnProjectile(PoolObjectType poolObjectType, float range, float damage, float velocity, float knockbackForce, float spread, GameObject attacker)
    {
        // Request an object pool
        PoolObject poolObj = ObjectPooler.GetInstance().RequestObject(poolObjectType);

        // Calculate direction with given spread deviation (randomed)
        Quaternion direction = Quaternion.Euler(
            transform.rotation.eulerAngles.x, 
            transform.rotation.eulerAngles.y, 
            transform.rotation.eulerAngles.z + Random.Range(-spread/2, spread/2)
            );

        // Activate fetched object
        Projectile projectile = poolObj.Activate(transform.position, direction).GetComponent<Projectile>();

        // Stats for projectile
        projectile.SetDamage(damage);
        projectile.SetRange(range);
        projectile.SetVelocity(velocity);
        projectile.SetKnockbackForce(knockbackForce);
        projectile.SetAttacker(attacker);
    }
}
