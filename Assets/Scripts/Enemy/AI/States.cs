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
        if (enemyAI.CheckDistance() < 0.5f)
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
        enemyAI.SetTarget(enemyAI.Player.position);
        enemyAI.SetSpeed(EnemyAI.MoveType.Idle);
    }
    public override void Exit(EnemyAI enemyAI)
    {

    }
    public override void Update(EnemyAI enemyAI)
    {
        float angel = enemyAI.CheckAngle();
        Debug.Log(angel);
        if (angel > 1f)
        {
            Debug.Log("Поверни");
            enemyAI.transform.Rotate(0, -2f, 0);
        }
        else if (angel < -1f)
        {
            Debug.Log("Поверни");
            enemyAI.transform.Rotate(0, 2f, 0);
        }
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
        if (enemyAI.CheckDistance() < 1f)
        {
            enemyAI.SetSpeed(EnemyAI.MoveType.Idle);
            enemyAI.SwitchState(enemyAI.Attack);
        }
    }
}
