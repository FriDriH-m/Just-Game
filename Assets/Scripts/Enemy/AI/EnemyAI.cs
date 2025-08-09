using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum MoveType
    {
        Walk = 3,
        Run = 6,
        Idle = 0
    }
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Animator _animator;
    private List<Transform> _shelters;
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
    public void SetSpeed(MoveType type)
    {
        _agent.speed = (int)type;
        switch (type)
        {
            case MoveType.Walk:
                SetAnimation("Walk", true);
                break;
            case MoveType.Run:
                SetAnimation("Walk", false);
                break;
            case MoveType.Idle:
                SetAnimation("Walk", false);
                break;
        }
    }
    public void FindNearilestShelter()
    {
        float closestDistance = 0;
        Vector3 closestTarget = Vector3.zero;
        for (int i = 0; i < _shelters.Count; i++)
        {
            if (_shelters[i] != null)
            {
                float distance = Vector3.Distance(transform.position, _shelters[i].position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = _shelters[i].position;
                }
            }
        }
        SetTarget(closestTarget);
        SetSpeed(MoveType.Run);
    }
    public float CheckDistance()
    {
        return (_target - transform.position).magnitude;
    }
    public Vector3 GetRandomPoint()
    {
        Vector3 randomPoint = Random.insideUnitSphere * 5f;
        randomPoint.y = 0f;
        return randomPoint;
    }
    public void SetAnimation(string name, bool value)
    {
        if (_animator != null)
        {
            _animator.SetBool(name, value);
        }
    }
    public void DetectShelter(Transform shelter, bool add)
    {
        if (add)
        {
            _shelters.Add(shelter);
        } else _shelters.Remove(shelter);
    }
    private void Update()
    {       
        _currentState.Update(this);        
    }
}
