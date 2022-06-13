using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HostageState
{
    IDLE, RUN
}
[RequireComponent(typeof(Animator))]
public class HostageAnimation : MonoBehaviour
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

    public void TriggerCheer()
    {
        anim.SetTrigger("Cheer");
    }
}
