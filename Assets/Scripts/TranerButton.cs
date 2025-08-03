using UnityEngine;

public class TranerButton : MonoBehaviour, IShootReaction
{
    [SerializeField] private GameObject _target;
    public void DoReaction()
    {
        _target.SetActive(!_target.activeSelf);
    }
}
