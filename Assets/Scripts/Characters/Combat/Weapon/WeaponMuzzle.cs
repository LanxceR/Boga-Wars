using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMuzzle : MonoBehaviour
{
    // Fire a projectile from this muzzle/fire position
    public void SpawnProjectile(PoolObjectType poolObjectType, float range, float damage, float velocity, float knockbackForce, GameObject attacker)
    {
        PoolObject poolObj = ObjectPooler.GetInstance().RequestObject(poolObjectType);
        Projectile projectile = poolObj.Activate(transform.position, transform.rotation).GetComponent<Projectile>();
        projectile.SetDamage(damage);
        projectile.SetRange(range);
        projectile.SetVelocity(velocity);
        projectile.SetKnockbackForce(knockbackForce);
        projectile.SetAttacker(attacker);
    }
}
