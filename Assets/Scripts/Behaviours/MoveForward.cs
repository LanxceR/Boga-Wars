using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Moveable))]
public class MoveForward : MonoBehaviour
{
    private Moveable moveable;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        moveable = GetComponent<Moveable>();
    }

    // This function is called when the object becomes enabled and active
    private void OnEnable()
    {
        // Move in vector (0,1,0) in respect of rotation (y axis/transform.up)
        moveable.SetDirection(transform.up);
    }
}
