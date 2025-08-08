using UnityEngine;


public class LimbDamage : MonoBehaviour, IShootReaction
{
    public enum LimbType
    {
        Head = 100,
        Body = 30,
        Limb = 20
    }
    private EnemyHealth _enemyHealth;
    [SerializeField] private LimbType _limbType;

    private void Start()
    {
        _enemyHealth = GetComponentInParent<EnemyHealth>();
    }

    public void DoReaction()
    {
        //Debug.Log($"Limb hit: {_limbType}");
        _enemyHealth?.TakeDamage(_limbType);
    }
}
