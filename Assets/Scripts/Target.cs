using UnityEngine;

public class Target : MonoBehaviour, IReactable
{
    private float _xRangeValue;
    private float _yRangeValue;
    private float _yRange
    {
        get => _yRangeValue;
        set => _yRangeValue = Mathf.Clamp(value, 1f, 4f);
    }
    
    private float _xRange
    {
        get => _xRangeValue;
        set => _xRangeValue = Mathf.Clamp(value, 2f, 26f);
    }

    public void DoReaction()
    {
        _yRange = Random.Range(1f, 5f);
        _xRange = Random.Range(2f, 27f);
        transform.position = new Vector3(_xRange, _yRange, transform.position.z);
    }
}
