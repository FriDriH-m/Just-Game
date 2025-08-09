using UnityEngine;

public abstract class EnemyBaseState
{
    public abstract void Enter(EnemyAI enemyAI);
    public abstract void Exit(EnemyAI enemyAI);
    public abstract void Update(EnemyAI enemyAI);
}

public class Patrol : EnemyBaseState
{
    private float _timer;
    public override void Enter(EnemyAI enemyAI)
    {

    }
    public override void Exit(EnemyAI enemyAI)
    {

    }
    public override void Update(EnemyAI enemyAI)
    {
        if (enemyAI.CheckDistance() < 0.1f)
        {
            enemyAI.SetSpeed(EnemyAI.MoveType.Idle);
        }
        _timer += Time.deltaTime;
        if (_timer >= 5f)
        {
            Vector3 randomPoint = enemyAI.GetRandomPoint();
            enemyAI.SetTarget(randomPoint);
            enemyAI.SetSpeed(EnemyAI.MoveType.Walk);
            _timer = 0f;
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
        enemyAI.FindNearilestShelter();
    }
    public override void Exit(EnemyAI enemyAI)
    {

    }
    public override void Update(EnemyAI enemyAI)
    {

    }
}
