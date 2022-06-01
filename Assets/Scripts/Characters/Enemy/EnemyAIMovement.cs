using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

[RequireComponent(typeof(Seeker), typeof(Rigidbody2D), typeof(BoxCollider2D))]
[RequireComponent(typeof(Moveable))]
public class EnemyAIMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D col;
    private Moveable movement;
    private Seeker seeker;
    private Path path;
    private RaycastHit2D hit;

    private int currentWaypoint;
    private bool reachedEndOfPath = false;
    private bool targetIsInSight = false;
    private float distanceToEnd;

    private float radius;

    [HideInInspector] public bool isMoving = false;

    [Header("Pathfinding Settings")]
    public Transform Target;
    [SerializeField] private LayerMask obstacleLayerMask;

    [SerializeField] private float endReachedDistance = 1f; // Useful for ranged characters to stop at a distance to shoot/attack
    [SerializeField] private float pathUpdateIntervalSeconds = 0.5f;
    [SerializeField] private float nextWaypointDistance = 3f;

    [Header("Movement Settings")]
    public float Speed = 3f; //Movespeed

    [Header("Misc Settings")]
    [SerializeField] private bool followTarget = true;
    [SerializeField] private bool canSeeTarget = true;
    [SerializeField] private bool drawGizmo = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        seeker = GetComponent<Seeker>();
        movement = GetComponent<Moveable>();

        InvokeRepeating(nameof(UpdatePath), 0f, pathUpdateIntervalSeconds);
    }

    // Update is called once per frame
    void Update()
    {
        // Set the active player in Game Manager as the target
        if (!Target)
            SetTarget(GameManager.GetInstance().ActivePlayer.transform);
    }

    // This function is called every fixed framerate frame, if the MonoBehaviour is enabled
    private void FixedUpdate()
    {
        if (Target)
        {
            if (canSeeTarget)
            {
                IsTargetInLineOfSight();
            }

            if (followTarget)
            {
                PathFollow();
            }
        }
    }

    // Set the target
    private void SetTarget(Transform target)
    {
        Target = target;
    }

    // Update/generate/calculate a new path
    private void UpdatePath()
    {
        if (seeker.IsDone() && followTarget && Target)
        {
            // Start path calculation
            seeker.StartPath(rb.position, Target.position, OnPathGenComplete);
        }
    }

    // Method to call when a path is generated/calculated
    private void OnPathGenComplete(Path p)
    {
        if (!p.error)
        {
            // Set path
            path = p;
            // Reset waypoint progress along the new path
            currentWaypoint = 0;
        }
    }    

    // Follow the path
    private void PathFollow()
    {
        // If there's no path, return
        if (path == null) return;

        // Has this object reached the destination yet (within this endReachedDistance radius, OR has reached the last waypoint in this path)
        distanceToEnd = Vector2.Distance(rb.position, Target.position);
        if ((distanceToEnd <= endReachedDistance && targetIsInSight) || (currentWaypoint >= path.vectorPath.Count))
        {
            movement.StopMoving();
            isMoving = false;
            reachedEndOfPath = true;
            return;
        }
        else
        {
            isMoving = true;
            reachedEndOfPath = false;
        }

        // Set direction for AI movement
        Vector2 dir = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;

        // Set moveable speed
        movement.SetSpeed(Speed);

        // Move using moveable
        movement.SetDirection(dir);

        // Has this object reached the current waypoint yet? 
        // (using a distance threshold nextWayPointDistance)
        float distanceToWaypoint = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distanceToWaypoint < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    // Check if target is in gameobject's "line of sight"
    private void IsTargetInLineOfSight()
    {
        // Perform a raycast from transform position to target's position
        radius = col.bounds.size.x > col.bounds.size.y ? col.bounds.size.x/2 : col.bounds.size.y/2;
        hit = Physics2D.CircleCast(transform.position, radius, DirectionToTarget(), distanceToEnd, obstacleLayerMask);

        // If raycast hit something, that means there's an obstacle in the way
        // If there's an obstacle in the way, target is NOT in line of sight
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
        if (Target)
        {
            Gizmos.DrawLine(transform.position, Target.position);
            Gizmos.DrawWireSphere(transform.position, radius);
        }

        // Stop distance
        Gizmos.color = new Color(166f / 255, 0f / 255, 255f / 255);
        Gizmos.DrawWireSphere(transform.position, endReachedDistance);
    }
}
