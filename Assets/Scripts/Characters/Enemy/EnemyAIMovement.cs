using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

[RequireComponent(typeof(Seeker), typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class EnemyAIMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D col;
    private Seeker seeker;
    private Path path;
    private RaycastHit2D hit;

    private int currentWaypoint;
    private bool reachedEndOfPath = false;
    private bool targetIsInSight = false;
    private float distanceToEnd;

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

        InvokeRepeating(nameof(UpdatePath), 0f, pathUpdateIntervalSeconds);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This function is called every fixed framerate frame, if the MonoBehaviour is enabled
    private void FixedUpdate()
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

    // Update/generate/calculate a new path
    private void UpdatePath()
    {
        if (seeker.IsDone() && followTarget)
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
            // set path
            path = p;
            // reset waypoint progress along the new path
            currentWaypoint = 0;
        }
    }    

    // Follow the path
    //
    // TODO : Fix reachedEndOfPath flag to avoid out of index exceptions (waypoint is still incremented if endReachedDistance < nextWayPointDistance)
    //
    private void PathFollow()
    {
        // If there's no path, return
        if (path == null) return;

        // Has this object reached the destination yet (within this endReachedDistance radius)
        distanceToEnd = Vector2.Distance(rb.position, Target.position);
        if (distanceToEnd <= endReachedDistance && targetIsInSight)
        {
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
        Vector2 dir = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized * Speed * Time.fixedDeltaTime;
        
        // Get next position
        Vector2 newPosition = rb.position + dir;

        // Move rigidbody
        rb.MovePosition(newPosition);

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
        hit = Physics2D.BoxCast(transform.position, col.bounds.size, 0, DirectionToTarget(), distanceToEnd, obstacleLayerMask);

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
        Gizmos.DrawLine(transform.position, Target.position);

        // Stop distance
        Gizmos.color = new Color(166f / 255, 0f / 255, 255f / 255);
        Gizmos.DrawWireSphere(transform.position, endReachedDistance);
    }
}
