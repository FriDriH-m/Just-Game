using UnityEngine;

public abstract class EnemyBaseState
{
    public abstract void Enter(EnemyAI enemyAI);
    public abstract void Exit(EnemyAI enemyAI);
    public abstract void Update(EnemyAI enemyAI);
}

public class Patrol : EnemyBaseState
{
    public override void Enter(EnemyAI enemyAI)
    {

    }
    public override void Exit(EnemyAI enemyAI)
    {

    }
    public override void Update(EnemyAI enemyAI)
    {
        if (enemyAI.ChecDistance() <= 5)
        {
            enemyAI.SwitchState(enemyAI._agro);
        }
    }
}
public class Attack : EnemyBaseState
{
    public override void Enter(EnemyAI enemyAI)
    {

    }
    public override void Exit(EnemyAI enemyAI)
    {

    }
    public override void Update(EnemyAI enemyAI)
    {

    }
}
public class Agro : EnemyBaseState
{
    public override void Enter(EnemyAI enemyAI)
    {
        enemyAI.SetTarget();
    }
    public override void Exit(EnemyAI enemyAI)
    {

    }
    public override void Update(EnemyAI enemyAI)
    {

    }
}
