using UnityEngine;


public interface IShootReaction
{
    public void DoReaction();
}
public interface IInputReaction
{
    public void TakeObject();
    public void DiscardObject();
}
public interface IInteractable
{
    public void Interact(Ray ray, RaycastHit hit);
    public void UnInteract(Ray ray, RaycastHit hit);
}

public class Flashlight : MonoBehaviour, IInputReaction, IInteractable
{
    [SerializeField] private Transform _targetPoint;
    [SerializeField] private GameObject _light;
    [SerializeField] private Outline _outline;
    private PlayerInputObserver _inputObserver;
    public void Interact(Ray ray, RaycastHit hit)
    {      
        if (_inputObserver == null) _inputObserver = PlayerInputObserver.Instance;
        _outline.enabled = true;
        _inputObserver.SubscribeToEvent(TakeObject, "Next");
    }
    public void UnInteract(Ray ray, RaycastHit hit)
    {
        _outline.enabled = false;
        _inputObserver.UnsubscribeFromEvent(TakeObject, "Next");
    }

    public void TakeObject()
    {
        _outline.enabled = false;

        // настройка объекта
        transform.GetComponent<Rigidbody>().isKinematic = true;
        transform.parent = _targetPoint;
        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;

        _inputObserver.SubscribeToEvent(OnOffLight, "Previous");

        _inputObserver.UnsubscribeFromEvent(TakeObject, "Next");
        _inputObserver.SubscribeToEvent(DiscardObject, "Next");
        
    }
    public void DiscardObject()
    {
        
        transform.parent = null;
        transform.GetComponent<Rigidbody>().isKinematic = false;

        _inputObserver.UnsubscribeFromEvent(OnOffLight, "Previous");
        _inputObserver.UnsubscribeFromEvent(DiscardObject, "Next");
    }
    public void OnOffLight()
    {
        _light.SetActive(!_light.activeInHierarchy);
    }
}
