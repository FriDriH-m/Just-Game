using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    private Transform _target;
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
    public void SetTarget(Transform target = null)
    {
        if (target != null)
        {
            _target = target;
            _agent.SetDestination(target.position);
        }
    }
    public void SetSpeed(float speed)
    {
        _agent.speed = speed;
    }
    public float ChecDistance()
    {
        return (_target.position - transform.position).magnitude;
    }
}
