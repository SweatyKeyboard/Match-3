using UnityEngine;
using UnityEngine.UI;

public class BonusButton : MonoBehaviour
{
    [SerializeField] private BonusEffect _bonusEffect;
    private Image _backImage;

    private void Awake()
    {
        _backImage = GetComponent<Image>();
    }
    public void ExecuteBonus()
    {
        Board.Instance.SetBonusAndWaitForTile(_bonusEffect.Bonus);
        _backImage.color = new Color(0.8f, 0.8f, 0.8f);
    }
}
