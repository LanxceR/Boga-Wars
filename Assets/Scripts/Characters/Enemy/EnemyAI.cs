using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

[RequireComponent(typeof(Seeker))]
public class EnemyAI : MonoBehaviour
{
    private Rigidbody2D rb;
    private Seeker seeker;
    private Path path;
    private int currentWaypoint;
    private bool reachedEndOfPath = false;

    [Header("Pathfinding Settings")]
    public Transform Target;
    [SerializeField] private float endReachedDistance = 1f; // Useful for ranged characters to stop at a distance to shoot/attack
    [SerializeField] private float pathUpdateIntervalSeconds = 0.5f;
    [SerializeField] private float nextWaypointDistance = 3f;

    [Header("Movement Settings")]
    public float Speed = 3f; //Movespeed

    [Header("Misc Settings")]
    [SerializeField] private bool followTarget = true;

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
    // TODO : Fix reachedEndOfPath to avoid out of index exceptions (waypoint is still incremented if endReachedDistance < nextWayPointDistance)
    //        Implement line of sight detection (for AI moving flags & can be used for other stuff e.g. firing a weapon)
    //
    private void PathFollow()
    {
        // If there's no path, return
        if (path == null) return;

        // Has this object reached the destination yet (within this endReachedDistance radius)
        float distanceToEnd = Vector2.Distance(rb.position, Target.position);
        if (distanceToEnd <= endReachedDistance)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
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
}
