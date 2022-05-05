using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveable : MonoBehaviour
{
    public float Speed = 1f;

    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }

    // Use to regularly update ship position, usually put in Update()
    private void UpdatePosition()
    {
        transform.position = GetNextPosition();
    }

    // Get the next position according to direction
    public Vector3 GetNextPosition()
    {
        return transform.position + NewPosition();
    }

    public Vector3 NewPosition()
    {
        return direction * Time.deltaTime * Speed;
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

    // Stop moving
    public void StopMoving()
    {
        SetDirection(Vector2.zero);
    }

    // Set specific axis direction, usually to reset direction to zero
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
