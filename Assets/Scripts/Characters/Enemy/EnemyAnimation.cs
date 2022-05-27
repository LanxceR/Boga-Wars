using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    IDLE, RUN, DEAD
}
[RequireComponent(typeof(Animator))]
public class EnemyAnimation : MonoBehaviour
{
    private Animator anim;

    [SerializeField] private LookAt lookAt;
    [SerializeField] private EnemyAIMovement enemyAIMove;
    [SerializeField] private HealthSystem health;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimationState();

        UpdateAnimationDirection();
    }

    public void TriggerHurt()
    {
        anim.SetTrigger("Hurt");
    }

    private void UpdateAnimationDirection()
    {
        if (lookAt)
        {
            Vector2 lookDirection = lookAt.transform.up;

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
        EnemyState state;


        if (health.isDead)
        {
            state = EnemyState.DEAD;
        }
        else if (enemyAIMove.isMoving)
        {
            state = EnemyState.RUN;
        }
        else
        {
            state = EnemyState.IDLE;
        }

        anim.SetInteger("State", (int)state);
    }
}
