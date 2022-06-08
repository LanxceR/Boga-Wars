using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorState
{
    CLOSE, OPEN
}
[RequireComponent(typeof(Animator))]
public class DoorAnimation : MonoBehaviour
{
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CloseDoor()
    {
        anim.SetInteger("State", (int)DoorState.CLOSE);
    }

    public void OpenDoor()
    {
        anim.SetInteger("State", (int)DoorState.OPEN);
    }
}
