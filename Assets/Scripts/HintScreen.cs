using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HintScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text _header;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private Image _image;

    public static HintScreen Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    public void SetData(Helper data)
    {
        _header.text = data.Header;
        _description.text = data.Description;
        _image.sprite = data.Image;
    }
}
