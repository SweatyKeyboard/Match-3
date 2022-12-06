using TMPro;
using UnityEngine;

public class CurseCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    private ICurse _curse = new IceCurse();

    private void Awake()
    {
        TurnsCounter.OnTurnsChanged += UpdateCounter;
    }
    private void OnDestroy()
    {
        TurnsCounter.OnTurnsChanged -= UpdateCounter;
    }
    private void UpdateCounter()
    {
        _text.text = (10 - (TurnsCounter.Instance.Turns % 10)).ToString();
        if (TurnsCounter.Instance.Turns % 10 == 0)
        {
            ExecuteCurse();
        }
    }

    private void ExecuteCurse()
    {
        _curse.Initialize(Board.Instance.TileViews);
    }
}
