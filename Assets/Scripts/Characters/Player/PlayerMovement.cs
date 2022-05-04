using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerMovement : MonoBehaviour
{
    private BoxCollider2D col;
    private Rigidbody2D rb;

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
        col = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
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
        Vector3 dir = new Vector2(MoveX, MoveY).normalized * Speed * Time.fixedDeltaTime;

        // Get next position
        Vector2 newPosition = transform.position + dir;

        // Move rigidbody
        rb.MovePosition(newPosition);
    }

    // OnMove listener from InputAction "PlayerInput.inputaction"
    void OnMove(InputValue moveValue)
    {
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
