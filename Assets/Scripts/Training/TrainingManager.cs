using UnityEngine;

public class TrainingManager : MonoBehaviour
{
    [SerializeField] private TranerButton[] _tranerButtons;
    [SerializeField] private DisplayUI _displayUI;
    [SerializeField] private Target _target;
    private Counter _counter;
    private TrainingObserver _trainingObserver;
    private bool _gameIsActive;
    private void Awake()
    {
        _counter = new Counter();
        _trainingObserver = new TrainingObserver();

        foreach (var item in _tranerButtons)
        {
            item.Init(_trainingObserver);
        }
        _counter.Init(_trainingObserver);
        _displayUI.Init(_counter);
        _target.Init(_trainingObserver);

        _trainingObserver.StartGame += StartGame;
        _trainingObserver.StopGame += StopGame;
        _trainingObserver.HitTarget += _counter.AddScore;
    }
    public void StartGame()
    {
        if (!_gameIsActive)
        {
            _target.ActiveTarget(true);
            _displayUI.ActiveDisplay();
            _counter.StartSession(this);
            _gameIsActive = true;
        }        
    }
    public void StopGame()
    {
        if (_gameIsActive)
        {
            _counter.Results(this);
            _target.ActiveTarget(false);
            _displayUI.DeactiveDisplay();            
            _gameIsActive = false;
        }
    }
    private void OnDestroy()
    {
        _trainingObserver.StartGame -= StartGame;
        _trainingObserver.StopGame -= StopGame;
        _trainingObserver.HitTarget -= _counter.AddScore;
    }
}
