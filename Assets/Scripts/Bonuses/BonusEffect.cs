using UnityEngine;

[CreateAssetMenu(menuName = "Match-3/Bonus")]
public class BonusEffect : ScriptableObject
{
    [SerializeField] private Sprite _icon;
    [SerializeField] private Bonuses _action;
    [SerializeField] private BonusPrice _price;
    [SerializeField] private Helper _helper;
    [SerializeField] private AudioClip _executeSound;

    public Helper Helper => _helper;

    public IBonus Bonus
    {
        get
        {
            IBonus bonus;
            switch (_action)
            {
                case Bonuses.RandomizeTile:
                    bonus = new RandomizingBonus();
                    break;

                case Bonuses.Explode9:
                    bonus = new Explode9();
                    break;

                case Bonuses.DestroyVertical:
                    bonus = new DestroyVertical();
                    break;

                case Bonuses.DestroyHorizontal:
                    bonus = new DestroyHorizontal();
                    break;

                case Bonuses.DestroyTile:
                    bonus = new DestroyTile();
                    break;

                case Bonuses.DestroyDiagonal1:
                    bonus = new DestroyDiagonal1();
                    break;

                case Bonuses.DestroyDiagonal2:
                    bonus = new DestroyDiagonal2();
                    break;

                case Bonuses.TripleTime:
                    bonus = new TripleTimeMark();
                    break;

                case Bonuses.TimeForType:
                    bonus = new TimeForTileType();
                    break;

                case Bonuses.DestroyCrossDiagonal:
                    bonus = new DestroyCrossDiagonal();
                    break;

                case Bonuses.DestroyCrossOrthogonal:
                    bonus = new DestroyCrossOrthogonal();
                    break;

                case Bonuses.MoveUp:
                    bonus = new MoveUp();
                    break;

                case Bonuses.MoveUpRight:
                    bonus = new MoveUpRight();
                    break;

                case Bonuses.MoveRight:
                    bonus = new MoveRight();
                    break;

                case Bonuses.MoveDownRight:
                    bonus = new MoveRightDown();
                    break;

                case Bonuses.MoveDown:
                    bonus = new MoveDown();
                    break;

                case Bonuses.MoveDownLeft:
                    bonus = new MoveDownLeft();
                    break;

                case Bonuses.MoveLeft:
                    bonus = new MoveLeft();
                    break;

                case Bonuses.MoveUpLeft:
                    bonus = new MoveLeftUp();
                    break;

                case Bonuses.DestroyTileType:
                    bonus = new DestroyTileType();
                    break;

                default:
                    return null;
            }

            return bonus;
        }
    }

    public AudioClip Sound => _executeSound;
    public BonusPrice Price => _price;
    public Sprite Icon => _icon;
}


