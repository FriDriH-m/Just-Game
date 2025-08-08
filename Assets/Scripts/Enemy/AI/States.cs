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
        _timer += Time.deltaTime;
        if (enemyAI.ChecDistance() <= 5)
        {
            enemyAI.SwitchState(enemyAI._agro);
        }
        if (_timer >= 5f)
        {
            Vector3 randomPoint = enemyAI.GetRandomPoint();
            enemyAI.SetTarget(randomPoint);
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
        
    }
    public override void Exit(EnemyAI enemyAI)
    {

    }
    public override void Update(EnemyAI enemyAI)
    {

    }
}
