using UnityEngine;
using System;

public class Target : MonoBehaviour, IShootReaction
{
    [SerializeField] private GameObject _persona;
    private TrainingObserver _observer;
    private float _xRangeValue;
    private float _yRangeValue;
    public void Init(TrainingObserver observer)
    {
        _observer = observer;
    }
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
    public void ActiveTarget(bool mode)
    {
        _persona.SetActive(mode);
        transform.gameObject.SetActive(mode);
    }
    public void DoReaction()
    {
        _persona.GetComponent<Animator>().SetTrigger("SeeMuscles");
        _observer.HitTarget?.Invoke();
        _yRange = UnityEngine.Random.Range(1f, 5f);
        _xRange = UnityEngine.Random.Range(2f, 27f);
        transform.position = new Vector3(_xRange, _yRange, transform.position.z);
    }
}
