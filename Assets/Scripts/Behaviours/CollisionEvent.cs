using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class CollisionEvent : MonoBehaviour
{
    [SerializeField] private string[] targetTags;

    [SerializeField] private UnityEvent onCollision;
    [SerializeField] private UnityEvent onCollisionExitEvent;
    [SerializeField] private UnityEvent onCollisionStayEvent;

    // Unity event that accepts a gameObject parameter
    // Mainly to access the other gameObject this object collides with
    [SerializeField] private UnityEvent<GameObject> onCollisionWithGameObject;
    [SerializeField] private UnityEvent<GameObject> onCollisionExitWithGameObject;
    [SerializeField] private UnityEvent<GameObject> onCollisionStayWithGameObject;

    // OnCollisionEnter is called when this collider/rigidbody has begun touching another rigidbody/collider
    private void OnCollisionEnter2D(Collision2D other)
    {
        foreach (string tag in targetTags)
        {
            //Debug.Log("Collision: " + name + ", other: " + other.name);
            // Check if object collided with a desired tagged object
            if (other.gameObject.CompareTag(tag))
            {
                // Invoke all method inside OnCollision event
                onCollision?.Invoke();
                onCollisionWithGameObject?.Invoke(other.gameObject);
            }
        }
    }

    // OnCollisionExit is called when this collider/rigidbody has stopped touching another rigidbody/collider
    private void OnCollisionExit2D(Collision2D other)
    {
        foreach (string tag in targetTags)
        {
            //Debug.Log("Collision: " + name + ", other: " + other.name);
            // Check if object collided with a desired tagged object
            if (other.gameObject.CompareTag(tag))
            {
                // Invoke all method inside OnCollision event
                onCollisionExitEvent?.Invoke();
                onCollisionExitWithGameObject?.Invoke(other.gameObject);
            }
        }
    }

    // OnCollisionStay is called once per frame for every collider/rigidbody that is touching rigidbody/collider
    private void OnCollisionStay2D(Collision2D other)
    {
        foreach (string tag in targetTags)
        {
            Debug.Log("Collision: " + name + ", other: " + other.gameObject.name);
            // Check if object collided with a desired tagged object
            if (other.gameObject.CompareTag(tag))
            {
                // Invoke all method inside OnCollision event
                onCollisionStayEvent?.Invoke();
                onCollisionStayWithGameObject?.Invoke(other.gameObject);
            }
        }
    }
}
