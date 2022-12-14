using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BonusButton : MonoBehaviour
{
    [SerializeField] private BonusEffect _bonusEffect;
    [SerializeField] private BonusCollection _collection;
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text[] _prices;
    [SerializeField] private int _unlocksSinceScore;

    [SerializeField] private AudioClip _clickSound;

    private Tile[] _priceTiles = new Tile[3];
    private Image _backImage;

    private bool _isToggled = false;
    private static bool _isAnyBonusSelected = false;

    private bool _isHelperShowed = false;
    private bool _isReleased = false;

    private void Start()
    {
        _backImage = GetComponent<Image>();
        GenerateBonus();
        GenereatePrice();
        ScoreCounter.Instance.OnScoreChanged += CheckUnlcok;
    }

    private void OnDestroy()
    {
        ScoreCounter.Instance.OnScoreChanged -= CheckUnlcok;
    }

    private void GenerateBonus()
    {
        _bonusEffect = _collection[Random.Range(0, _collection.Count)];
        CheckUnlcok();
    }

    private void CheckUnlcok()
    {
        if (_unlocksSinceScore <= ScoreCounter.Instance.Score)
        {
            _icon.sprite = _bonusEffect.Icon;
            _backImage.color = Color.white;
        }
    }
    private void GenereatePrice()
    {
        int zeroes = _bonusEffect.Price.GetZeroesCount();
        List<Tile> possibleTiles = Board.Instance.PossibleTiles;

        for (int i = 0; i < 3 - zeroes; i++)
        {
            int randomIndex = Random.Range(0, possibleTiles.Count);
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
                _prices[1].color = _priceTiles[1].Color;
                _prices[2].color = _priceTiles[2].Color;
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
        _isAnyBonusSelected = false;
        Board.Instance.CancelBonus();
        _backImage.color = Color.white;
        _isToggled = false;
        Board.Instance.OnBonusEnd -= Unclick;

        if (isSucceeded)
        {
            for (int i = 0; i < 3 - _bonusEffect.Price.GetZeroesCount(); i++)
            {
                _priceTiles[i].Mana -= _bonusEffect.Price.Prices[i];
            }
        }
    }

    private void Click()
    {
        AudioPlayer.PlaySound(_clickSound);

        _isAnyBonusSelected = true;
        Board.Instance.OnBonusEnd += Unclick;
        Board.Instance.SetBonusAndWaitForTile(_bonusEffect.Bonus, _bonusEffect.Sound);
        _backImage.color = new Color(0.6f, 0.7f, 0.6f);
        _isToggled = true;        
    }


    public void ExecuteBonus()
    {
            if (IsEnoughMana())
            {
                if (!_isToggled && !_isAnyBonusSelected)
                {
                    Click();
                }
                else
                {
                    Unclick(false);
                }
            }
    }


    private void OnMouseDown()
    {
        if (_unlocksSinceScore <= ScoreCounter.Instance.Score)
        {
            StartCoroutine(ShowHelpWithPause(1f));
            _isReleased = false;
        }
    }

    private void OnMouseUp()
    {
        if (_unlocksSinceScore <= ScoreCounter.Instance.Score)
        {
            StopAllCoroutines();
            _isReleased = true;
            if (!_isHelperShowed)
            {
                ExecuteBonus();
            }
            else
            {
                HintScreen.Instance.gameObject.SetActive(false);
                _isHelperShowed = false;
            }
        }
    }

    IEnumerator ShowHelpWithPause(float holdDurationToShow)
    {
        yield return new WaitForSeconds(holdDurationToShow);
        if (!_isReleased)
        {
            _isHelperShowed = true;
            HintScreen.Instance.gameObject.SetActive(true);
            HintScreen.Instance.SetData(_bonusEffect.Helper);
        }
    }
}
