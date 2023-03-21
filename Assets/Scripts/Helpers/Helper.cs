using UnityEngine;

[CreateAssetMenu(menuName = "Match-3/Hint")]
public class Helper : ScriptableObject
{
    [SerializeField] private string _header;
    [SerializeField] [Multiline] private string _description;
    [SerializeField] private Sprite _image;

    public string Header => _header;
    public string Description => _description;
    public Sprite Image => _image;
}
