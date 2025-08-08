using UnityEngine;


public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private int _health = 100;
    public int Health => _health;
    
    public void TakeDamage(LimbDamage.LimbType limbType)
    {
        _health -= (int)limbType;
        if (_health <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        _animator.SetTrigger("Death");
        //gameObject.SetActive(false);
    }
}
