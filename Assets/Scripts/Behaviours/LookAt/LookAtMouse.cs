using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LookAtMouse : MonoBehaviour
{
    private Vector2 mousePosition;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.GetInstance().IsPlaying) return;

        // Flip sprite based on parent's euler angle (rotation in degrees)
        // 180 < x < 360  ==>  Aiming left
        // 0 < x < 180  ==>  Aiming right
        if (180f < transform.eulerAngles.z && transform.eulerAngles.z < 360f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        } 
        else
        {

            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    // OnLook listener from InputAction "PlayerInput.inputaction"
    void OnLook(InputValue mousePos)
    {
        if (!GameManager.GetInstance().IsPlaying) return;

        // Get mouse position on screen
        mousePosition = mousePos.Get<Vector2>();

        // Translate screen position to world position
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePosition);

        // "Rotate" this gameobject up axis
        transform.up = new Vector3(worldPos.x - transform.position.x, worldPos.y - transform.position.y);
    }
}
