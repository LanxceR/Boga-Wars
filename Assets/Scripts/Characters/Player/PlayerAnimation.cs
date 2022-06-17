using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    IDLE, RUN, DEAD
}
[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;

    [Header("Components")]
    [SerializeField] private LookAtMouse mouseLook;
    [SerializeField] private PlayerMovement playerMove;
    [SerializeField] private HealthSystem health;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.GetInstance().IsPlaying) return;

        UpdateAnimationState();

        UpdateAnimationDirection();
    }

    public void TriggerHurt()
    {
        anim.SetTrigger("Hurt");
    }

    private void UpdateAnimationDirection()
    {
        if (mouseLook)
        {
            Vector2 lookDirection = mouseLook.transform.up;

            anim.SetFloat("DirX", lookDirection.x);
            anim.SetFloat("DirY", lookDirection.y);
        }
        else
        {
            // TODO: Update player sprite facing based off of movement
        }
    }

    private void UpdateAnimationState()
    {
        PlayerState state;


        if (health.isDead)
        {
            state = PlayerState.DEAD;
        }
        else if (playerMove.MoveX != 0 || playerMove.MoveY != 0)
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
