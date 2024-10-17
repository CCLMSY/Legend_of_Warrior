public class Boar : Enemy
{
    protected override void Awake()
    {
        base.Awake();
        patrolState = new BoarPatrolState();
        chaseState = new BoarChaseState();
    }
    // public override void Move()
    // {
    //     base.Move();
    //     anim.SetBool("walk", true);
    // }
}
