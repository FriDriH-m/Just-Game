using UnityEngine;

public class Flashlight : MonoBehaviour
{
    private PlayerInputObserver _inputObserver;
    private GameObject _light;

    public void Initialize(PlayerInputObserver inputObserver, GameObject light)
    {
        _light = light;
        _inputObserver = inputObserver;
        _inputObserver.SubscribeToEvent(OnOffLight, "Previous");
    }
    public void OnOffLight()
    {
        _light.SetActive(!_light.activeInHierarchy);
    }
}
