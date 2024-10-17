using UnityEngine;

public class BeePatrolState : BaseState
{
    private Vector3 target;
    private Vector3 moveDir;
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
        target = enemy.GetNewPoint();
    }

    public override void LogicUpdate()
    {
        if (currentEnemy.FoundPlayer())
        {
            currentEnemy.SwitchState(NPCState.Chase);
        }

        if (Vector3.Distance(currentEnemy.transform.position, target) < 0.1f)
        {
            currentEnemy.isWaiting = true;
            target = currentEnemy.GetNewPoint();
        }

        moveDir = (target - currentEnemy.transform.position).normalized;

        if (moveDir.x > 0) currentEnemy.transform.localScale = new Vector3(-1, 1, 1);
        if (moveDir.x < 0) currentEnemy.transform.localScale = new Vector3(1, 1, 1);

    }

    public override void PhysicsUpdate()
    {
        if (!currentEnemy.isWaiting && !currentEnemy.isDead && !currentEnemy.isHurt)
        {
            currentEnemy.rb.velocity = moveDir * currentEnemy.currentSpeed * Time.deltaTime;
        }
        else
        {
            currentEnemy.rb.velocity = Vector2.zero;
        }
    }

    public override void OnExit()
    {
        // currentEnemy.anim.SetBool("walk", false);
    }
}
