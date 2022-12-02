using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BonusButton : MonoBehaviour
{
    [SerializeField] private BonusEffect _bonusEffect;
    [SerializeField] private TMP_Text[] _prices;

    private Tile[] _priceTiles = new Tile[3];
    private Image _backImage;

    private bool _isToggled = false;

    private void Awake()
    {
        _backImage = GetComponent<Image>();
        Board.Instance.OnBonusEnd += Unclick;
        GenereatePrice();
    }
    private void OnDestroy()
    {
        Board.Instance.OnBonusEnd -= Unclick;
    }

    private void GenereatePrice()
    {
        int zeroes = _bonusEffect.Price.GetZeroesCount();
        List<Tile> possibleTiles = Board.Instance.PossibleTiles;

        for (int i = 0; i < 3 - zeroes; i++)
        {
            int randomIndex = Random.Range(0, _priceTiles.Length);
            _priceTiles[i] = possibleTiles[randomIndex];
            possibleTiles.RemoveAt(randomIndex);
        }

        switch (zeroes)
        {
            case 0:
                _prices[0].text = _bonusEffect.Price.Prices[0].ToString();
                _prices[1].text = _bonusEffect.Price.Prices[1].ToString();
                _prices[2].text = _bonusEffect.Price.Prices[2].ToString();

                _prices[0].color = _priceTiles[0].Color;
                _prices[2].color = _priceTiles[1].Color;
                break;

            case 1:
                _prices[0].text = _bonusEffect.Price.Prices[0].ToString();
                _prices[1].text = "";
                _prices[2].text = _bonusEffect.Price.Prices[1].ToString();

                _prices[0].color = _priceTiles[0].Color;
                _prices[2].color = _priceTiles[1].Color;
                break;

            case 2:
                _prices[0].text = "";
                _prices[1].text = _bonusEffect.Price.Prices[0].ToString();
                _prices[2].text = "";

                _prices[1].color = _priceTiles[0].Color;
                break;
        }
    }

    private bool IsEnoughMana()
    {
        bool isEnough = true;
        for (int i = 0; i < 3 - _bonusEffect.Price.GetZeroesCount(); i++)
        {
            if (_priceTiles[i].Mana < _bonusEffect.Price.Prices[i])
            {
                isEnough = false;
            }
        }
        return isEnough;
    }

    private void Unclick(bool isSucceeded)
    {
        Board.Instance.CancelBonus();
        _backImage.color = new Color(1f, 1f, 1f);
        _isToggled = false;

        for (int i = 0; i < 3 - _bonusEffect.Price.GetZeroesCount(); i++)
        {
            _priceTiles[i].Mana -= _bonusEffect.Price.Prices[i];
        }
    }

    private void Click()
    {
        Board.Instance.SetBonusAndWaitForTile(_bonusEffect.Bonus);
        _backImage.color = new Color(0.6f, 0.7f, 0.6f);
        _isToggled = true;        
    }


    public void ExecuteBonus()
    {
        if (IsEnoughMana())
        {
            if (!_isToggled)
            {
                Click();
            }
            else
            {
                Unclick(false);
            }
        }
    }
}
