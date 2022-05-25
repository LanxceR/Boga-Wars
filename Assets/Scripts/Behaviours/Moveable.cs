using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveable : MonoBehaviour
{
    private float Speed = 1f;
    private Vector3 direction;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!rb)
            UpdatePosition();
    }

    // This function is called every fixed framerate frame, if the MonoBehaviour is enabled
    private void FixedUpdate()
    {
        if (rb)
            FixedUpdatePosition();
    }

    // Use to regularly update transform position, usually put in Update()
    private void UpdatePosition()
    {
        transform.position = GetNextPosition();
    }
    // Use to regularly update rigidbody position with physics, usually put in FixedUpdate()
    private void FixedUpdatePosition()
    {
        rb.MovePosition(GetNextPosition());
    }

    // Get the next position according to direction
    public Vector3 GetNextPosition()
    {
        return transform.position + NewPosition();
    }

    public Vector3 NewPosition()
    {
        if (!rb)
            return direction.normalized * Time.deltaTime * Speed;
        else
            return direction.normalized * Time.fixedDeltaTime * Speed;
    }

    // Set movement direction
    public void SetDirection(Vector3 value)
    {
        direction = value;
    }
    public void SetDirection(Vector2 value)
    {
        direction.y = value.y;
        direction.x = value.x;
    }
    public void SetDirection(float x, float y)
    {
        direction.y = y;
        direction.x = x;
    }

    public void SetSpeed(float speed)
    {
        Speed = speed;
    }

    // Stop moving
    public void StopMoving()
    {
        SetDirection(Vector2.zero);
    }

    // Set specific axis direction
    internal void SetXDirection(float xValue)
    {
        direction.x = xValue;
    }
    internal void SetYDirection(float yValue)
    {
        direction.y = yValue;
    }

    // Skew direction by adding a new direction component
    internal void AddDirection(Vector3 value)
    {
        direction += value;
    }
    internal void AddDirection(Vector2 value)
    {
        direction.y += value.y;
        direction.x += value.x;
    }
    internal void AddDirection(float x, float y)
    {
        direction.y += y;
        direction.x += x;
    }
}
