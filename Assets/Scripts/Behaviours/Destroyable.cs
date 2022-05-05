using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    [Header("Main Setting")]
    [SerializeField] private GameObject Parent; // Optional

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
    public void DestroyObject()
    {
        Destroy(gameObject);
    }

    public void DeactivateParent()
    {
        Parent.SetActive(false);
    }
    public void DestroyObjectParent()
    {
        Destroy(Parent);
    }
}
