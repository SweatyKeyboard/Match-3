using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(TMP_Text))]
public class Timer : MonoBehaviour
{
    private const float STARTTIME = 150f;
    private static float _time;
    private TMP_Text _text;
    private bool _isStoped = false;

    public static Timer Instance { get; private set; }
    public float SecondsLeft
    {
        get => _time;
        set
        {
            _time = value;
            UpdateText();
        }
    }

    private string FormattedTime => TimeSpan.FromSeconds(_time).ToString(@"mm\:ss");

    public event Action OnTimerEnd;

    private void Awake()
    {
        _time = STARTTIME;
        Instance = this;
        _text = GetComponent<TMP_Text>();
        UpdateText();
    }

    private void Update()
    {
        if (!_isStoped)
        {
            _time -= Time.deltaTime;
            UpdateText();

            if (_time <= 0)
            {
                Debug.Log("Time!");
                OnTimerEnd?.Invoke();
                _isStoped = true;
            }
        }  
    }
    private void UpdateText()
    {
        _text.text = FormattedTime;
    }

}
