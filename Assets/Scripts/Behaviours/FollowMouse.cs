using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FollowMouse : MonoBehaviour
{
    private Vector2 mousePosition;

    [SerializeField] private Transform rootTransform;
    [SerializeField] private float maxRadius;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.GetInstance().IsPlaying) return;

        // Translate screen position to world position
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        Vector2 worldPos = ray.GetPoint(0.5f);

        if (rootTransform)
        {
            // Find the point of the mousepos relative to rootTransform
            // And clamp the position with a max radius relative to rootTransform
            Vector2 difference = worldPos - (Vector2)rootTransform.position;
            float magnitude = difference.magnitude;
            if (magnitude > maxRadius)
            {
                difference = difference * (maxRadius / magnitude);
            }

            // Move this object's position relative to rootTransform
            transform.localPosition = difference;
        }
        else
        {
            // Move this object's position in world space
            transform.position = worldPos;
        }
    }

    // OnLook listener from InputAction "PlayerInput.inputaction"
    void OnLook(InputValue mousePos)
    {
        if (!GameManager.GetInstance().IsPlaying) return;

        // Get mouse position on screen
        mousePosition = mousePos.Get<Vector2>();
    }
}
