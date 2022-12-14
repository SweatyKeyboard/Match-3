using System.Collections.Generic;
using UnityEngine;

public class TileView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private ParticleSystem _destroyParticles;

    [SerializeField] private Transform _iceLayer; 
    [SerializeField] private Transform _timeLayer;
    [SerializeField] private Transform _arrowLayer;

    [SerializeField] private AudioClip _clickSound;


    private Tile _tile;
    private TileCurses _curses = new TileCurses();
    private TileEffects _effects = new TileEffects();

    private float _scaleInTheBounds = 0.8f;

    private bool _isLocked = false;
    
    public bool IsAlreadyMoved { get; set; }

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

    public TileCurses Curses
    {
        get => _curses;
        set
        {
            _curses = value;
        }
    }

    public TileEffects Effects
    {
        get => _effects;
        set
        {
            _effects = value;
            CheckEffects();
        }
    }


    private void Awake()
    {
        _curses.OnChanged += CheckCurses;
        _effects.OnChanged += CheckEffects;
    }

    private void OnDestroy()
    {
        _curses.OnChanged -= CheckCurses;
        _effects.OnChanged -= CheckEffects;
    }

    private void OnMouseDown()
    {
        if (Board.Instance.State == BoardStates.Game)
        {
            if (!_isLocked)
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
        }
        else if (Board.Instance.State == BoardStates.BonusAwaiting)
        {
            Board.Instance.ExecuteBonus(this);
        }
    }
    public void CheckCurses()
    {
        _isLocked = _curses.IsIced;
        _iceLayer.gameObject.SetActive(_curses.IsIced);

        _arrowLayer.gameObject.SetActive(_curses.MovingDirection != Vector2.zero);
        _arrowLayer.rotation = Quaternion.FromToRotation(Vector2.right, _curses.MovingDirection);
    }

    private void ClearCurses()
    {
        _curses.ClearCurses();
        CheckCurses();
    }

    private void CheckEffects()
    {
        _timeLayer.gameObject.SetActive(_effects.IsTriplingTime);
    }

    private void ClearEffects()
    {
        _effects.Clear();
        CheckEffects();
    }

    private void Select()
    {
        _spriteRenderer.color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
        _previousTile = this;
        AudioPlayer.PlaySound(_clickSound);
    }

    private void Deselect()
    {
        _spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        _previousTile = null;
        AudioPlayer.PlaySound(_clickSound);
    }

    private void TrySwap()
    {
        if (Board.Instance.GetAllAdjacentTiles(_previousTile).Contains(this))
        {
            Board.Instance.Swap(this, _previousTile);
            _previousTile.CheckCurses();
            Board.Instance.FindAllMatchesForTile(_previousTile);
            _previousTile.Deselect();
            CheckCurses();
            Board.Instance.FindAllMatchesForTile(this);
            
        }
        else
        {
            _previousTile.Deselect();
            Select();
        }
    }

    public Vector2 MovingDirection()
    {
        return Curses.MovingDirection;
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

        ClearCurses();
        ClearEffects();
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
