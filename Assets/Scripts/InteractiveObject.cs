using UnityEngine;

public interface IReactable
{
    public void DoReaction();
}

public class InteractiveObject : MonoBehaviour, IReactable
{
    [SerializeField] private Material _material;    
    [SerializeField] private GameObject _target;
    private bool _isActive;

    private void Start()
    {
        _material.color = Color.red;
    }

    public void DoReaction()
    {
        if (_isActive)
        {
            _isActive = false;
            _material.color = Color.red;
        }
        else
        {
            _isActive = true;
            _material.color = Color.green;
        }
        
        _target.SetActive(!_target.activeInHierarchy);
    }
}
