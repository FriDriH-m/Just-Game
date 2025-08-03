using UnityEngine;

public class CameraLook
{
    private Camera _camera;
    private IInteractable _currentInteractable;

    public void Initialize(Camera camera)
    {
        _camera = camera;
    }

    public void CameraLooking()
    {        
        Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.distance > 3f) return; 
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();           

            if (interactable != null && interactable != _currentInteractable)
            {
                //Debug.Log("Looked");
                interactable.Interact(ray, hit);
                _currentInteractable?.UnInteract(ray, hit);
                _currentInteractable = interactable;
            }
            if (interactable == null)
            {
                //Debug.Log("No Looking");
                _currentInteractable?.UnInteract(ray, hit);
                _currentInteractable = null;
            }
        }
    }
}
