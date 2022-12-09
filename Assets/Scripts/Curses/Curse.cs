using UnityEngine;

[CreateAssetMenu(menuName = "Match-3/Curse")]
public class Curse : ScriptableObject
{
    [SerializeField] private Sprite _image;
    [SerializeField] private Helper _helper;
    [SerializeField] private Curses _curse;

    public Sprite Image => _image;
    public Helper Helper => _helper;
    public Curses CurseType => _curse;
}
