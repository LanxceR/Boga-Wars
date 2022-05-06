using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    IDLE, RUN
}
public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private LookAtMouse weapon;
    private PlayerMovement playerMove;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        playerMove = GetComponent<PlayerMovement>();
        weapon = GetComponentInChildren<LookAtMouse>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimationState();

        if (weapon)
        {
            Vector2 weaponRotation = weapon.transform.up;

            anim.SetFloat("DirX", weaponRotation.x);
            anim.SetFloat("DirY", weaponRotation.y);
        }
        else
        {
            // TODO: Update player sprite facing based off of movement
        }
    }

    private void UpdateAnimationState()
    {
        PlayerState state;


        if (playerMove.MoveX != 0 || playerMove.MoveY != 0)
        {
            state = PlayerState.RUN;
        }
        else
        {
            state = PlayerState.IDLE;
        }

        anim.SetInteger("State", (int)state);
    }
}
