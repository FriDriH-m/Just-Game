using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    private Vector3 _target;
    private EnemyBaseState _currentState;
    public Agro _agro { get; private set; }
    public Attack _attack { get; private set; }
    public Patrol _patrol { get; private set; }

    private void Awake()
    {
        _agro = new();
        _attack = new();
        _patrol = new();
    }
    private void Start()
    {
        SwitchState(_patrol);
    }
    public void SwitchState(EnemyBaseState newState)
    {
        if (_currentState != null) _currentState.Exit(this);
        _currentState = newState;
        _currentState.Enter(this);
    }
    public void SetTarget(Vector3 target)
    {
        if (target != null)
        {
            _target = target;
            Debug.DrawLine(transform.position, target, Color.red, (_target - transform.position).magnitude);
            _agent.SetDestination(target);
        }
    }
    public void SetSpeed(float speed)
    {
        _agent.speed = speed;
    }
    public float ChecDistance()
    {
        return (_target - transform.position).magnitude;
    }
    public Vector3 GetRandomPoint()
    {
        Vector3 randomPoint = Random.insideUnitSphere * 5f;
        randomPoint.y = 0f;
        return randomPoint;
    }
    private void Update()
    {       
        _currentState.Update(this);        
    }
}
