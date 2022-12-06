using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Match-3/Bonus")]
public class BonusEffect : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _icon;
    [SerializeField] private Bonuses _action;
    [SerializeField] private BonusPrice _price;

    public IBonus Bonus
    {
        get
        {
            switch(_action)
            {
                case Bonuses.RandomizeTile:
                    return new RandomizingBonus();

                case Bonuses.Explode9:
                    return new Explode9();

                case Bonuses.DestroyVertical:
                    return new DestroyVertical();

                case Bonuses.DestroyHorizontal:
                    return new DestroyHorizontal();

                case Bonuses.DestroyTile:
                    return new DestroyTile();

                default:
                    return null;
            }
        }
    }
    public BonusPrice Price => _price;
    public Sprite Icon => _icon;
}


