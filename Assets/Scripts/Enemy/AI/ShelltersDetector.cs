using UnityEngine;

public class SheltersDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Shelter"))
        {
            EnemyAI enemyAI = GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                enemyAI.DetectShelter(other.transform, true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Shelter"))
        {
            EnemyAI enemyAI = GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                enemyAI.DetectShelter(other.transform, false);
            }
        }
    }
}
