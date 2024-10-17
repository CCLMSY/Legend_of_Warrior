public abstract class BaseState // 有限状态机的基类
{
    protected Enemy currentEnemy; // 状态所描述的Enemy
    public abstract void OnEnter(Enemy enemy); // 进入状态时调用
    public abstract void LogicUpdate(); // 逻辑更新
    public abstract void PhysicsUpdate(); // 物理更新
    public abstract void OnExit(); // 退出状态时调用
}
