using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityGLTF.Interactivity.Schema;

public class EnemyAI : MonoBehaviour
{
    public enum MoveType
    {
        Walk = 3,
        Run = 10,
        Idle = 0
    }
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _player;
    private List<Transform> _shelters;
    private Vector3 _target;
    private EnemyBaseState _currentState;
    private Agro _agro;
    private Attack _attack;
    private Patrol _patrol; 
    public Attack Attack => _attack;
    public Patrol Patrol => _patrol;
    public Agro Agro => _agro;
    public EnemyBaseState CurrentState => _currentState;
    public Transform Player => _player;

    //----------------Unity Methods------------------
    private void Awake()
    {
        _agro = new();
        _attack = new();
        _patrol = new();
    }
    private void Start()
    {
        SwitchState(_patrol);
        _shelters = new List<Transform>();
    }
    private void Update()
    {
        _currentState.Update(this);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_target, 0.5f);
    }
    //-------------------------------------------------
    //----------------Default Methods------------------
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
            _agent.SetDestination(target);
        }
    }
    public float CheckDistance()
    {
        return (_target - transform.position).magnitude;
    }
    public float CheckAngle()
    {
        Vector3 vectorToTarget = (_target - transform.position).normalized;
        vectorToTarget.y = 0;        

        Vector3 enemyForward = transform.forward.normalized;
        enemyForward.y = 0;

        return Vector3.SignedAngle(enemyForward, vectorToTarget, Vector3.up);
    }
    public void SetAnimation(string name, bool value)
    {
        if (_animator != null)
        {
            _animator.SetBool(name, value);
        }
    }
    public void SetSpeed(MoveType type)
    {
        _agent.speed = (int)type;
        switch (type)
        {
            case MoveType.Walk:
                SetAnimation("Run", false);
                SetAnimation("Walk", true);
                break;
            case MoveType.Run:
                SetAnimation("Run", true);
                SetAnimation("Walk", false);
                break;
            case MoveType.Idle:
                SetAnimation("Run", false);
                SetAnimation("Walk", false);
                break;
        }
    }
    //------------------------------------------   
    //--------------Shelter Modul---------------
    public void DetectShelter(Transform shelter, bool add)
    {
        if (add)
        {
            _shelters.Add(shelter);
        }
        else _shelters.Remove(shelter);
    }
    public void FindNearilestShelter()
    {
        float closestDistance = 100;
        Transform closestShelter = null;
        for (int i = 0; i < _shelters.Count; i++)
        {
            float distance = Vector3.Distance(transform.position, _shelters[i].position);

            if (distance < closestDistance && !_shelters[i].GetComponent<Shelter>().IsBusy)
            {
                closestDistance = distance;
                closestShelter = _shelters[i];
            }
        }   
        if (closestShelter == null)
        {
            SwitchState(_attack);
            return;
        }
        
        closestShelter.GetComponent<Shelter>().ChoseShelter(true);
        SetTarget(closestShelter.position);
        SetSpeed(MoveType.Run);
    }
    //----------------------------------------    
    public Vector3 GetRandomPoint()
    {
        Vector3 randomPoint = Random.insideUnitSphere * 5f;
        randomPoint.y = 0f;
        return transform.position + randomPoint;
    }
}
