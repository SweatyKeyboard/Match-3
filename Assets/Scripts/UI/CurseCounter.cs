using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurseCounter : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Curse[] _curses;

    private Helper _helper;
    private ICurse _curse;
    private int curseTimer = 10;

    private bool _isReleased = false;

    private void Awake()
    {
        TurnsCounter.OnTurnsChanged += UpdateCounter;
        PickCurse();
    }

    private void OnDestroy()
    {
        TurnsCounter.OnTurnsChanged -= UpdateCounter;
    }

    private void UpdateCounter()
    {
        _text.text = (curseTimer - (TurnsCounter.Instance.Turns % curseTimer)).ToString();
        if (TurnsCounter.Instance.Turns % curseTimer == 0)
        {
            ExecuteCurse();
            PickCurse();
        }
    }

    private void PickCurse()
    {
        int index = Random.Range(0, _curses.Length);
        Curse curse = _curses[index];

        switch(curse.CurseType)
        {
            case Curses.Ice:
                _curse = new IceCurse();
                break;

            case Curses.Arrow:
                _curse = new MovingCurse();
                break;
        }

        _image.sprite = curse.Image;
        _helper = curse.Helper;
    }

    private void ExecuteCurse()
    {
        _curse.Initialize(Board.Instance.TileViews);
    }

    private void OnMouseDown()
    {
        StartCoroutine(ShowHelpWithPause(1f));
        _isReleased = false;
    }    

    private void OnMouseUp()
    {
        StopAllCoroutines();
        _isReleased = true;
        HintScreen.Instance.gameObject.SetActive(false);
    }

    IEnumerator ShowHelpWithPause(float holdDurationToShow)
    {
        yield return new WaitForSeconds(holdDurationToShow);
        if (!_isReleased)
        {
            HintScreen.Instance.gameObject.SetActive(true);
            HintScreen.Instance.SetData(_helper);
        }
    }
}