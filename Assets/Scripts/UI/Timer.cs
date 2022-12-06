using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(TMP_Text))]
public class Timer : MonoBehaviour
{
    private static float _time = 60;
    private TMP_Text _text;

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

    private void Awake()
    {
        Instance = this;
        _text = GetComponent<TMP_Text>();
        UpdateText();
    }

    private void Update()
    {
        _time -= Time.deltaTime;
        UpdateText();
    }
    private void UpdateText()
    {
        _text.text = FormattedTime;
    }

}
