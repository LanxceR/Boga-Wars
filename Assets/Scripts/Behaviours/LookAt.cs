using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    public Transform Target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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

        LookAtTarget();
    }

    // Look at target (rotate gameobject towards target)
    private void LookAtTarget()
    {
        // "Rotate" this gameobject up axis
        transform.up = Target.position - transform.position;
    }
}
