using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    [SerializeField] private string[] targetTags;

    [SerializeField] private UnityEvent onTrigger;
    [SerializeField] private UnityEvent onTriggerExitEvent;
    [SerializeField] private UnityEvent onTriggerStayEvent;

    // Unity event that accepts a gameObject parameter
    // Mainly to access the other gameObject this object collides with
    [SerializeField] private UnityEvent<GameObject> onTriggerWithGameObject; 
    [SerializeField] private UnityEvent<GameObject> onTriggerExitWithGameObject;
    [SerializeField] private UnityEvent<GameObject> onTriggerStayWithGameObject;

    // OnTriggerEnter2D is called when the Collider2D other enters the trigger (2D physics only)
    private void OnTriggerEnter2D(Collider2D other)
    {
        foreach (string tag in targetTags)
        {
            //Debug.Log("Collision: " + name + ", other: " + other.name);
            // Check if object collided with a desired tagged object
            if (other.gameObject.CompareTag(tag))
            {
                // Invoke all method inside OnCollision event
                onTrigger?.Invoke();
                onTriggerWithGameObject?.Invoke(other.gameObject);
            }
        }
    }

    // OnTriggerExit2D is called when the Collider2D other has stopped touching the trigger (2D physics only)
    private void OnTriggerExit2D(Collider2D other)
    {
        foreach (string tag in targetTags)
        {
            //Debug.Log("Collision: " + name + ", other: " + other.name);
            // Check if object collided with a desired tagged object
            if (other.gameObject.CompareTag(tag))
            {
                // Invoke all method inside OnCollision event
                onTriggerExitEvent?.Invoke();
                onTriggerExitWithGameObject?.Invoke(other.gameObject);
            }
        }
    }

    // OnTriggerStay2D is called once per frame for every Collider2D other that is touching the trigger (2D physics only)
    private void OnTriggerStay2D(Collider2D other)
    {
        foreach (string tag in targetTags)
        {
            //Debug.Log("Collision: " + name + ", other: " + other.name);
            // Check if object collided with a desired tagged object
            if (other.gameObject.CompareTag(tag))
            {
                // Invoke all method inside OnCollision event
                onTriggerStayEvent?.Invoke();
                onTriggerStayWithGameObject?.Invoke(other.gameObject);
            }
        }
    }
}
