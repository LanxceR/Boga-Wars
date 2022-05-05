using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Collider2D col; // To disable collider when exploding
    private Moveable movement;
    private Vector2 startingPosition;
    private float range = 5f;
    private bool hit;
    private float damage = 1f;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        col = GetComponentInChildren<Collider2D>();
        movement = GetComponent<Moveable>();
    }

    // This function is called when the object becomes enabled and active
    private void OnEnable()
    {
        startingPosition = transform.position;
        hit = false;
        col.enabled = true;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOutOfRange() && !HasHit())
        {
            movement.StopMoving();
        }
    }

    // Set projectile range (lifetime based on dist. travelled)
    public void SetRange(float range)
    {
        this.range = range;
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public void SetVelocity(float velocity)
    {
        movement.Speed = velocity;
    }

    // Check if projectile had travelled its range distance
    public bool IsOutOfRange()
    {
        return Vector2.Distance(startingPosition, transform.position) > range;
    }

    // Check if the projectile had hit something
    public bool HasHit()
    {
        return hit;
    }
    // Set Hit condition
    public void SetHit(bool hit)
    {
        this.hit = hit;
    }
}
