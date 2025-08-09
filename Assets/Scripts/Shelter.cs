using UnityEngine;

public class Shelter : MonoBehaviour
{
    private bool _isBusy = false;
    public bool IsBusy => _isBusy;
    public void ChoseShelter(bool isBusy)
    {
        _isBusy = isBusy;
    }
}
