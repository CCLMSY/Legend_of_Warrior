using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeChaseState : BaseState
{
    private Attack attack;
    private Vector3 target;
    private Vector3 moveDir;
    private bool isChase;
    private float attackRateCounter = 0;
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        attack = currentEnemy.GetComponent<Attack>();

        currentEnemy.lostCounter = currentEnemy.lostTime;

        currentEnemy.anim.SetBool("chase", true);
    }

    public override void LogicUpdate()
    {
        if (currentEnemy.lostCounter <= 0){
            currentEnemy.SwitchState(NPCState.Patrol);
        }

        target = new Vector3(currentEnemy.attacker.position.x, currentEnemy.attacker.position.y + 1.5f, 0);

        if (Vector2.Distance(target, currentEnemy.transform.position) <= attack.attackRange)
        {
            isChase = true;
            // if(!currentEnemy.isHurt) currentEnemy.rb.velocity = Vector2.zero;

            attackRateCounter -= Time.deltaTime;
            if (attackRateCounter <= 0)
            {
                currentEnemy.anim.SetTrigger("attack");
                attackRateCounter = attack.attackRate;
            }
        }
        else
        { //超出攻击范围
            isChase = false;
        }

        moveDir = (target - currentEnemy.transform.position).normalized;

        if (moveDir.x > 0) currentEnemy.transform.localScale = new Vector3(-1, 1, 1);
        if (moveDir.x < 0) currentEnemy.transform.localScale = new Vector3(1, 1, 1);

    }

    public override void PhysicsUpdate()
    {
        if (!currentEnemy.isDead && !currentEnemy.isHurt && !isChase)
        {
            currentEnemy.rb.velocity = moveDir * currentEnemy.currentSpeed * Time.deltaTime;
        }
        else
        {
            if (!currentEnemy.isHurt) currentEnemy.rb.velocity = Vector2.zero;
        }
    }

    public override void OnExit()
    {
        currentEnemy.anim.SetBool("chase", false);
    }
}
