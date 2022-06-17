using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Moveable))]
public class PlayerMovement : MonoBehaviour
{
    private Moveable movement;

    [Header("Movement Settings")]
    public float Speed = 5f; //Movespeed

    [Header("Layer Masks")]
    [SerializeField] private LayerMask wallLayerMask;
    [SerializeField] private LayerMask actorLayerMask;

    public float MoveX { get; private set; }
    public float MoveY { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<Moveable>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // This function is called every fixed framerate frame, if the MonoBehaviour is enabled
    // Move Player rigidbody in FixedUpdate
    private void FixedUpdate()
    {
        // Set direction vector for player movement
        Vector2 dir = new Vector2(MoveX, MoveY);

        // Set moveable speed
        movement.SetSpeed(Speed);

        // Move using moveable
        movement.SetDirection(dir);
    }

    // OnMove listener from InputAction "PlayerInput.inputaction"
    void OnMove(InputValue moveValue)
    {
        if (!GameManager.GetInstance().IsPlaying) return;

        // Get input value
        Vector2 moveVector = moveValue.Get<Vector2>();

        // Value ranges between -1 and 1
        // -1    => left, down
        // 1     => right, up
        MoveX = moveVector.x;
        MoveY = moveVector.y;
    }

    // Converts given bitmask to layer number
    public static int ToLayer(int bitmask)
    {
        int result = bitmask > 0 ? 0 : 31;
        while (bitmask > 1)
        {
            bitmask = bitmask >> 1;
            result++;
        }
        return result;
    }
}
