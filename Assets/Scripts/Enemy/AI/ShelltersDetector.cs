using UnityEngine;

public class SheltersDetector : MonoBehaviour
{
    EnemyAI _enemyAI;
    private void Start()
    {
        _enemyAI = GetComponentInParent<EnemyAI>();
        if (_enemyAI == null)
        {
            Debug.LogError("EnemyAI component not found on parent object.");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Shelter>())
        {
            if (_enemyAI != null)
            {
                _enemyAI.DetectShelter(other.transform, true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Shelter>())
        {
            if (_enemyAI != null)
            {
                _enemyAI.DetectShelter(other.transform, false);
            }
        }
    }
}
