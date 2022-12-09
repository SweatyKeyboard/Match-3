using UnityEngine;

[CreateAssetMenu(menuName = "Match-3/Bonus")]
public class BonusEffect : ScriptableObject
{
    [SerializeField] private Sprite _icon;
    [SerializeField] private Bonuses _action;
    [SerializeField] private BonusPrice _price;
    [SerializeField] private Helper _helper;

    public Helper Helper => _helper;

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

                case Bonuses.DestroyDiagonal1:
                    return new DestroyDiagonal1();

                case Bonuses.DestroyDiagonal2:
                    return new DestroyDiagonal2();

                case Bonuses.TripleTime:
                    return new TripleTimeMark();

                case Bonuses.TimeForType:
                    return new TimeForTileType();

                case Bonuses.DestroyCrossDiagonal:
                    return new DestroyCrossDiagonal();

                case Bonuses.DestroyCrossOrthogonal:
                    return new DestroyCrossOrthogonal();

                case Bonuses.MoveUp:
                    return new MoveUp();

                case Bonuses.MoveUpRight:
                    return new MoveUpRight();

                case Bonuses.MoveRight:
                    return new MoveRight();

                case Bonuses.MoveDownRight:
                    return new MoveRightDown();

                case Bonuses.MoveDown:
                    return new MoveDown();

                case Bonuses.MoveDownLeft:
                    return new MoveDownLeft();

                case Bonuses.MoveLeft:
                    return new MoveLeft();

                case Bonuses.MoveUpLeft:
                    return new MoveLeftUp();

                case Bonuses.DestroyTileType:
                    return new DestroyTileType();

                default:
                    return null;
            }
        }
    }
    public BonusPrice Price => _price;
    public Sprite Icon => _icon;
}


