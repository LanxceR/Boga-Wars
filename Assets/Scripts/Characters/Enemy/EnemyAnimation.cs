using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    IDLE, RUN
}
[RequireComponent(typeof(Animator))]
public class EnemyAnimation : MonoBehaviour
{
    private Animator anim;

    [SerializeField] private LookAt lookAt;
    [SerializeField] private EnemyAIMovement enemyAIMove;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimationState();

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


        if (enemyAIMove.isMoving)
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
