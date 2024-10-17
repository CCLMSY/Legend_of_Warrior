using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarChaseState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        // Debug.Log("Chase");
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        currentEnemy.anim.SetBool("run", true);

        currentEnemy.lostCounter = currentEnemy.lostTime;
    }

    public override void LogicUpdate()
    {
        if (currentEnemy.lostCounter <= 0)
            currentEnemy.SwitchState(NPCState.Patrol);

        if (!currentEnemy.physicsCheck.isGrounded || currentEnemy.physicsCheck.touchLeftWall)
        {
            currentEnemy.transform.localScale = new Vector3(currentEnemy.faceDir.x, 1, 1);
        }
    }

    public override void PhysicsUpdate()
    {

    }

    public override void OnExit()
    {
        currentEnemy.anim.SetBool("run", false);
    }
}
