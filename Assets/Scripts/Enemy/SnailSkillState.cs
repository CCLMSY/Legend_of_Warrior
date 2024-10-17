using UnityEngine;

public class SnailSkillState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = 0;
        currentEnemy.anim.SetBool("walk", false);
        currentEnemy.anim.SetBool("hide", true);
        currentEnemy.anim.SetTrigger("skill");

        currentEnemy.lostCounter = currentEnemy.lostTime;
        currentEnemy.GetComponent<Character>().isInvincible = true;
        currentEnemy.GetComponent<Character>().invincibleCounter = currentEnemy.lostCounter;
    }

    public override void LogicUpdate()
    {

        if (currentEnemy.lostCounter <= 0)
        {
            currentEnemy.SwitchState(NPCState.Patrol);
        }
        currentEnemy.GetComponent<Character>().invincibleCounter = currentEnemy.lostCounter;

    }

    public override void PhysicsUpdate()
    {

    }

    public override void OnExit()
    {
        currentEnemy.anim.SetBool("hide", false);
        currentEnemy.GetComponent<Character>().isInvincible = false;
    }

}
