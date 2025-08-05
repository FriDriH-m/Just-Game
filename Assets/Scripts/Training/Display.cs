using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DisplayUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _time;
    [SerializeField] private TextMeshProUGUI _score;
    [SerializeField] private TextMeshProUGUI _recordText;
    [SerializeField] private TextMeshProUGUI _results;
    private Counter _counter;

    public void Init(Counter counter)
    {
        _counter = counter;
        _counter.TimerEvent += UpdateDisplay;
    }

    public void ActiveDisplay()
    {
        _recordText.gameObject.SetActive(false);
        _time.gameObject.SetActive(true);
        _score.gameObject.SetActive(true);
        _results.gameObject.SetActive(false);
    }
    public void DeactiveDisplay()
    {
        _recordText.gameObject.SetActive(true);
        _recordText.text = "Record: " + _counter._record.ToString();
        _time.gameObject.SetActive(false);
        _score.gameObject.SetActive(false);
        _results.gameObject.SetActive(true);
        _results.text = "Your score: " + _counter._results.ToString();
    }
    private void UpdateDisplay()
    {
        _score.text = "Score: " + _counter._score.ToString();
        _time.text = "Time: " + _counter._time.ToString();
    }
}
public class Counter
{
    public Action TimerEvent;
    public int _record { get; private set; }
    public int _time { get; private set; }
    public int _score { get; private set; }
    public int _results { get; private set; }
    private Coroutine _timer;
    private TrainingObserver _trainingObserver;
    public void Init(TrainingObserver trainingObserver)
    {
        _trainingObserver = trainingObserver;
    }
    public void StartSession(MonoBehaviour monoBeh)
    {
        _time = 30;
        _score = 0;
        if (_timer == null)
        {
            _timer = monoBeh.StartCoroutine(Timer());
        }
        else return;
    }
    public void AddScore()
    {
        _score++;
    }
    public void Results(MonoBehaviour monoBehaviour)
    {
        monoBehaviour.StopCoroutine(_timer);
        _timer = null;
        _time = 0;
        _results = _score;
        if (_score > _record)
        {
            _record = _score;
        }
    }
    public IEnumerator Timer()
    {
        for (int i = _time; i >= 0; i--)
        {
            _time = i;
            TimerEvent?.Invoke();
            yield return new WaitForSeconds(1f);
        }
        _trainingObserver.StopGame?.Invoke();
        _timer = null;
        yield break;
    }
}
