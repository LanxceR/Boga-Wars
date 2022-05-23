using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    private Weapon[] weapons;
    private RaycastHit2D hit;

    private bool canShoot;
    private bool targetIsInSight = false;
    private float distanceToPlayer;

    [Header("Main Settings")]
    public Transform Target;
    [SerializeField] private Transform weaponAttachPosition;
    [SerializeField] private LayerMask obstacleLayerMask;
    [SerializeField] private float shootDistance = 7f; // Preferably smaller than weapon range

    [Header("Misc Settings")]
    [SerializeField] private bool canShootTarget = true;
    [SerializeField] private bool drawGizmo = false;

    // Start is called before the first frame update
    void Start()
    {
        // Fetch all weapons
        weapons = GetComponentsInChildren<Weapon>();
    }

    // Update is called once per frame
    void Update()
    {
        // Set the active player in Game Manager as the target
        if (!Target)
            SetTarget(GameManager.GetInstance().ActivePlayer.transform);

        if (canShootTarget && Target)
        {
            IsTargetInLineOfSight();
            TryToShoot();
        }
    }

    // Set the target
    private void SetTarget(Transform target)
    {
        Target = target;

        foreach (Weapon w in weapons)
        {
            // Set the target for each weapon if it hasn't
            LookAt weaponLook = w.GetComponent<LookAt>();
            if (!weaponLook.Target)
                weaponLook.SetTarget(Target);
        }
    }

    // Try to shoot the target
    private void TryToShoot()
    {
        // Distance to target
        distanceToPlayer = Vector2.Distance(transform.position, Target.position);

        // Is the target within shoot distance?
        if (distanceToPlayer <= shootDistance && targetIsInSight)
        {
            canShoot = true;
        }
        else
        {
            canShoot = false;
        }

        // Can this gameobject shoot the target?
        if (canShoot)
        {
            foreach (Weapon w in weapons)
            {
                // Try to fire all weapons
                w.Attack();
            }
        }
    }

    // Check if player is in gameobject's "line of sight"
    private void IsTargetInLineOfSight()
    {
        // Perform a raycast from transform position to target's position
        hit = Physics2D.Raycast(weaponAttachPosition.position, DirectionToTarget(), distanceToPlayer, obstacleLayerMask);

        // If raycast hit something, that means there's an obstacle in the way
        // If there's an obstacle in the way, player is NOT in line of sight
        if (hit)
            targetIsInSight = false;
        else
            targetIsInSight = true;
    }

    // Find direction in Vector3 to the target
    private Vector3 DirectionToTarget()
    {
        return Target.position - transform.position;
    }

    // Implement this OnDrawGizmosSelected if you want to draw gizmos only if the object is selected
    private void OnDrawGizmos()
    {
        if (!drawGizmo) return;

        // Line of sight
        if (targetIsInSight)
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, Target.position);

        // Weapon range
        Gizmos.color = new Color(224f / 255, 230f / 255, 46f / 255);
        Gizmos.DrawWireSphere(transform.position, shootDistance);
    }
}
