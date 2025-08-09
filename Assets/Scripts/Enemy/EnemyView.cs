using UnityEngine;

public class EnemyView : MonoBehaviour
{
    EnemyAI _enemyAI;
    private void Start()
    {
        _enemyAI = GetComponentInParent<EnemyAI>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_enemyAI != null && _enemyAI.CurrentState == _enemyAI.Patrol)
            {
                _enemyAI.SwitchState(_enemyAI.Agro);
            } 
        }
    }
}
