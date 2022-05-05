using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enum for pool object type
public enum PoolObjectType
{
    PLAYER_SHURIKEN,
    GENERIC_PLAYER_PROJECTILE,
    GENERIC_ENEMY_PROJECTILE,
    EXPLOSION
}
public class PoolObject : MonoBehaviour
{
    [Header("Main Setting")]
    public PoolObjectType ObjectType;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        Deactivate();
    }

    // Activate pool object in hierarchy
    public void Activate(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
    }
    public void Activate(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
        gameObject.SetActive(true);
    }

    // Deactivate pool object in hierarchy
    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    // Check if this pool object is active in hierarchy or not
    internal bool IsActive()
    {
        return gameObject.activeInHierarchy;
    }
}
