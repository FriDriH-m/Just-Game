using UnityEngine;

public class TranerButton : MonoBehaviour, IShootReaction
{
    [SerializeField] private GameObject _target;
    [SerializeField] private Display _display;
    public void DoReaction()
    {
        _target.SetActive(!_target.activeSelf);
        if (_target.activeSelf)
        {
            _display.ActiveDisplay();
        }
        else
        {
            _display.DeactiveDisplay();
        }
        
    }
}
