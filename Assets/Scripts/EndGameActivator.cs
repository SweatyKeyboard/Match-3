using UnityEngine;

public class EndGameActivator : MonoBehaviour
{
    [SerializeField] private EndGame _endGame;

    private void Start()
    {
        Timer.Instance.OnTimerEnd += _endGame.EndThisGame;
        _endGame.Deactivate();
    }

    private void OnDestroy()
    {
        Timer.Instance.OnTimerEnd -= _endGame.EndThisGame;
    }
}
