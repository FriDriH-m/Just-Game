using UnityEngine;
using System;

public class TranerButton : MonoBehaviour, IShootReaction
{
    public enum ButtonType
    {
        Start,
        Stop,
        SeeRecords
    }
    private TrainingObserver _observer;
    [SerializeField] private ButtonType _buttonType;
    public void Init(TrainingObserver trainingObserver)
    {
        _observer = trainingObserver;
    }
    public void DoReaction()
    {
        switch (_buttonType)
        {
            case ButtonType.Start:
                _observer.StartGame?.Invoke();
                break;
            case ButtonType.Stop:
                _observer.StopGame?.Invoke();
                break;
            case ButtonType.SeeRecords:
                _observer.SeeRecords?.Invoke();
                break;
            default:
                break;                
        }
    }
}

public class TrainingObserver
{
    public Action StartGame;
    public Action StopGame;
    public Action SeeRecords;
    public Action HitTarget;
}
