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
    [SerializeField] private GameObject _takeObjectCanvas;
    private PlayerInputObserver _inputObserver;
    private GameObject _light;
    private bool _inHand = false;
    
    public void Initialize(GameObject light)
    {
        _light = light;
        _inputObserver = PlayerInputObserver.Instance; 
    }
    public void Interact(Ray ray, RaycastHit hit)
    {      
        _takeObjectCanvas.SetActive(true);
        _takeObjectCanvas.transform.position = hit.point - ray.direction * 0.5f;
        _takeObjectCanvas.transform.rotation = Quaternion.LookRotation(ray.direction, Vector3.up);
        _inputObserver.SubscribeToEvent(TakeObject, "Next");
    }
    public void UnInteract(Ray ray, RaycastHit hit)
    {
        _takeObjectCanvas.SetActive(false);
        _inputObserver.UnsubscribeFromEvent(TakeObject, "Next");
    }

    public void TakeObject()
    {
        if (_inHand) return;
        _inHand = true;

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
        if (!_inHand) return;
        _inHand = false;
        
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
