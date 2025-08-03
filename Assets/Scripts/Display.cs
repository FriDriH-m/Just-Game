using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Display : MonoBehaviour
{
    [SerializeField] private GameObject _scoreAndTime;
    [SerializeField] private TextMeshProUGUI _time;
    [SerializeField] private TextMeshProUGUI _score;
    [SerializeField] private TextMeshProUGUI _recordText;
    [SerializeField] private GameObject _record;
    [SerializeField] private TranerButton _button;
    private int _recordValue = 0;
    private int _timer;
    private int _scoreValue;
    private Coroutine _trainingCoroutine;
    public void ActiveDisplay()
    {
        _record.SetActive(false);
        _scoreAndTime.SetActive(true);
        _timer = 60;
        _scoreValue = 0;
        _trainingCoroutine = StartCoroutine(Training());
    }
    public void DeactiveDisplay()
    {
        if (_scoreValue > _recordValue)
        {
            _recordValue = _scoreValue;
        }
        _scoreValue = 0;
        _record.SetActive(true);
        _recordText.text = "Record: " + _recordValue.ToString();
        _scoreAndTime.SetActive(false);
        _timer = 0;
        StopCoroutine(_trainingCoroutine);
    }
    public void UpScore()
    {
        _scoreValue++;
    }
    private void Update()
    {
        _score.text = "Score: " + _scoreValue.ToString();
    }
    public IEnumerator Training()
    {
        for (int i = 0; i < 60; i++)
        {
            yield return new WaitForSeconds(1);
            _timer--;
            _time.text = "Time: " + _timer.ToString();
        }
        _button.DoReaction();
    }
}
