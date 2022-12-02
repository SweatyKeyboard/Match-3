using System.Collections.Generic;
using UnityEngine;

public class TileView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private ParticleSystem _destroyParticles;

    private Tile _tile;
    private float _scaleInTheBounds = 0.8f;

    private static TileView _previousTile;
    public Tile Tile 
    {
        get => _tile;
        set
        {
            _tile = value;
            UpdateView();
        }
    }

    private void OnMouseDown()
    {
        if (Board.Instance.State == BoardStates.Game)
        {
            if (_previousTile != this)
            {
                if (_previousTile == null)
                {
                    Select();
                }
                else
                {
                    TrySwap();
                }
            }
            else
            {
                Deselect();
            }
        }
        else if (Board.Instance.State == BoardStates.BonusAwaiting)
        {
            Board.Instance.ExecuteBonus(this);
        }
    }

    private void Select()
    {
        _spriteRenderer.color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
        _previousTile = this;
    }

    private void Deselect()
    {
        _spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        _previousTile = null;
    }

    private void TrySwap()
    {
        if (Board.Instance.GetAllAdjacentTiles(_previousTile).Contains(this))
        {
            Board.Instance.Swap(this, _previousTile);
            Board.Instance.FindAllMatchesForTile(_previousTile);
            _previousTile.Deselect();
            Board.Instance.FindAllMatchesForTile(this);
        }
        else
        {
            _previousTile.Deselect();
            Select();
        }
    }

    private void UpdateView()
    {
        _spriteRenderer.sprite = Tile?.Sprite;      
    }

    public void Scale(float scale)
    {
        transform.localScale = new Vector3(scale, scale) * _scaleInTheBounds;
    }

    public void Destroy()
    {
        
        ParticleSystem particles = Instantiate(_destroyParticles.gameObject, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
        var particlesMain = particles.main;
        particlesMain.startColor = Tile.Color;

        Tile = null;
    }

    public static void DeselectAll()
    {
        _previousTile?.Deselect();
    }

    public void RandomizeType(List<Tile> possibleTiles)
    {
        Tile = possibleTiles[Random.Range(0, possibleTiles.Count)];
    }


}
