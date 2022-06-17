using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Rigidbody2D))]
public class MoveableObstacle : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D col;

    // Start is called just before any of the Update methods is called the first time
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    private void FixedUpdate()
    {
        if (rb.velocity.magnitude > 0.01f)
        {
            UpdateGraphs();
        }
    }

    // Use to update A* graph on this part of bounds (for example, when this object has been moved)
    public void UpdateGraphs()
    {
        // Use the bounding box from the attached collider
        Bounds bounds = col.bounds;
        var guo = new GraphUpdateObject(bounds);

        // Set some settings
        guo.updatePhysics = true;

        // Update graphs
        AstarPath.active.UpdateGraphs(guo);
    }
}
