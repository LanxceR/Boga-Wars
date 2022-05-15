using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

[RequireComponent(typeof(Seeker))]
public class EnemyAIMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Seeker seeker;
    private Path path;
    private RaycastHit2D hit;

    private int currentWaypoint;
    private bool reachedEndOfPath = false;
    private bool playerIsInSight = false;
    private float distanceToEnd;

    [HideInInspector] public bool isMoving = false;

    [Header("Pathfinding Settings")]
    public Transform Target;
    [SerializeField] private LookAt look; // For line of sight detection
    [SerializeField] private LayerMask obstacleLayerMask;

    [SerializeField] private float endReachedDistance = 1f; // Useful for ranged characters to stop at a distance to shoot/attack
    [SerializeField] private float pathUpdateIntervalSeconds = 0.5f;
    [SerializeField] private float nextWaypointDistance = 3f;

    [Header("Movement Settings")]
    public float Speed = 3f; //Movespeed

    [Header("Misc Settings")]
    [SerializeField] private bool drawGizmo = false;
    [SerializeField] private bool followTarget = true;
    [SerializeField] private bool canSeePlayer = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
        if (canSeePlayer)
        {
            IsPlayerInLineOfSight();
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
        if (distanceToEnd <= endReachedDistance && playerIsInSight)
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

    // Check if player is in gameobject's "line of sight"
    private void IsPlayerInLineOfSight()
    {
        // Perform a raycast from look's root transform position to target's position
        hit = Physics2D.Raycast(look.transform.position, look.transform.up, distanceToEnd, obstacleLayerMask);

        // If raycast hit something, that means there's an obstacle in the way
        // If there's an obstacle in the way, player is NOT in line of sight
        if (hit)
            playerIsInSight = false;
        else
            playerIsInSight = true;
    }

    // Implement this OnDrawGizmosSelected if you want to draw gizmos only if the object is selected
    private void OnDrawGizmos()
    {
        if (!drawGizmo) return;

        // Line of sight
        if (playerIsInSight) 
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, Target.position);

        // Stop distance
        Gizmos.color = new Color(166f / 255, 0f / 255, 255f / 255);
        Gizmos.DrawWireSphere(transform.position, endReachedDistance);
    }
}
