using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
internal class Board : MonoBehaviour
{
    [SerializeField] private Tile[] _tiles;
    [SerializeField] private int _xSize, _ySize;
    [SerializeField] private TileView _tilePrefab;
    [SerializeField] private AudioClip _matchSound;

    private TileView[,] _tileViews;
    private RectTransform rectTransform;

    private IBonus _bonus;
    private AudioClip _bonusSound;

    private float _width;
    private float _height;
    private float _xScale;
    private float _yScale;

    private bool _isMatchFound;
    private int _findNullsMethodExecutedAtOnceCounter = 0;

    public static Board Instance { get; private set; }
    public TileView[,] TileViews => _tileViews;
    public BoardStates State { get; private set; }
    public List<Tile> PossibleTiles => _tiles.ToList();

    public event System.Action<bool> OnBonusEnd;


    private void Awake()
    {
        Instance = this;

        rectTransform = GetComponent<RectTransform>();
        _width = rectTransform.rect.width;
        _height = rectTransform.rect.height;

        CreateBoard();
    }

    private void CreateBoard()
    {
        _tileViews = new TileView[_xSize, _ySize];

        _xScale = _width / _xSize;
        _yScale = _height / _ySize;


        for (int x = 0; x < _xSize; x++)
        {
            for (int y = 0; y < _ySize; y++)
            {
                List<Tile> possibleTiles = new List<Tile>();
                possibleTiles.AddRange(_tiles);

                if (x > 1)
                {
                    if (_tileViews[x - 1, y].Tile == _tileViews[x - 2, y].Tile)
                    {
                        possibleTiles.Remove(_tileViews[x - 1, y].Tile);
                    }
                }

                if (y > 1)
                {
                    if (_tileViews[x, y - 1].Tile == _tileViews[x, y - 2].Tile)
                    {
                        possibleTiles.Remove(_tileViews[x, y - 1].Tile);
                    }
                }

                TileView newTile = Instantiate(
                    _tilePrefab,
                    transform.position,
                    Quaternion.identity,
                    transform);
                newTile.RandomizeType(possibleTiles);
                newTile.Scale(Mathf.Min(_xScale, _yScale));
                newTile.transform.localPosition = new Vector3(
                        (_xScale * x - _width / 2 + _xScale / 2),
                        (_yScale * y - _height / 2 + _yScale / 2));
                _tileViews[x, y] = newTile;
            }
        }
    }

    public void Swap(TileView tile1, TileView tile2, bool isProhobitedToDoNotMatch = true)
    {
        bool isChangingCurses = false;

        Tile tempTile = tile1.Tile;
        tile1.Tile = tile2.Tile;
        tile2.Tile = tempTile;

        if (isProhobitedToDoNotMatch)
        {
            if (!IsThereMatches())
            {
                tempTile = tile1.Tile;
                tile1.Tile = tile2.Tile;
                tile2.Tile = tempTile; 
            }
            else
            {
                isChangingCurses = true;
            }
        }
        else
        {
            isChangingCurses = true;
        }

        if (isChangingCurses)
        {
            TileCurses tempCurses = tile1.Curses;
            tile1.Curses = tile2.Curses;
            tile2.Curses = tempCurses;
        }
    }

    private void CheckAllTileCurses()
    {
        foreach (TileView tile in _tileViews)
        {
            tile.CheckCurses();
        }
    }

    private TileView GetAdjacent(TileView tileView, Vector2 direction)
    {
        tileView.gameObject.SetActive(false);
        RaycastHit2D hit = Physics2D.Raycast(tileView.transform.position, direction);
        tileView.gameObject.SetActive(true);


        TileView tempTileView;
        if (hit.collider != null &&
            hit.collider.TryGetComponent(out tempTileView))
        {
            return tempTileView;
        }
        return null;
    }

    public List<TileView> GetAllAdjacentTiles(TileView tileView)
    {
        Vector2[] adjacentDirections = new Vector2[]
        {
            Vector2.left,
            Vector2.up,
            Vector2.right,
            Vector2.down
        };
        List<TileView> adjacentTiles = new List<TileView>();
        for (int i = 0; i < adjacentDirections.Length; i++)
        {
            adjacentTiles.Add(GetAdjacent(tileView, adjacentDirections[i]) ?? tileView);
        }
        return adjacentTiles;
    }

    private void FillNulls()
    {
        StopCoroutine(FindNullTiles());
        StartCoroutine(FindNullTiles());
    }

    private bool IsThereMatches(bool andDestroyThem = false)
    {
        _isMatchFound = false;
        for (int x = 0; x < _xSize; x++)
        {
            for (int y = 0; y < _ySize; y++)
            {
                FindAllMatchesForTile(_tileViews[x, y], andDestroyThem);
            }
        }
        return _isMatchFound;
    }


    private List<TileView> CheckMatchForTileInDirection(TileView tileView, Vector2 direction)
    {
        List<TileView> matchingTiles = new List<TileView>();
        tileView.gameObject.SetActive(false);
        RaycastHit2D hit = Physics2D.Raycast(tileView.transform.position, direction);
        tileView.gameObject.SetActive(true);

        TileView tempTileView;
        while (
            hit.collider != null &&
            hit.collider.TryGetComponent(out tempTileView) &&
            tempTileView.Tile == tileView.Tile)
        {
            matchingTiles.Add(hit.collider.GetComponent<TileView>());
            Collider2D currentTile = hit.collider;
            currentTile.gameObject.SetActive(false);
            hit = Physics2D.Raycast(hit.collider.transform.position, direction);
            currentTile.gameObject.SetActive(true);
        }
        return matchingTiles;
    }

    private void FindMatchesForTile(TileView tileView, Vector2[] paths, bool isDestroyingFound = true)
    {
        List<TileView> matchingTiles = new List<TileView>();
        for (int i = 0; i < paths.Length; i++)
        {
            matchingTiles.AddRange(CheckMatchForTileInDirection(tileView, paths[i]));
        }
        matchingTiles.Add(tileView);

        if (matchingTiles.Count >= 3)
        {
            _isMatchFound = true;

            if (isDestroyingFound)
            {
                _isMatchFound = true;

                AudioPlayer.PlaySoundWithRandomPitch(_matchSound);

                int timeModifier = 1;
                foreach (TileView matchingTile in matchingTiles)
                {
                    if (matchingTile.Effects.IsTriplingTime)
                    {
                        timeModifier *= 3;
                    }
                }

                tileView.Tile.Mana += matchingTiles.Count - 2;
                ScoreCounter.Instance.Score += matchingTiles.Count - 2;
                Timer.Instance.SecondsLeft += (matchingTiles.Count - 2) * timeModifier;

                ClearMatch(matchingTiles);
            }
        }
    }

    public void FindAllMatchesForTile(TileView tileView, bool isDestroynigFound = true)
    {

        if (tileView.Tile == null)
            return;

        FindMatchesForTile(tileView, new Vector2[2] { Vector2.left, Vector2.right }, isDestroynigFound);
        FindMatchesForTile(tileView, new Vector2[2] { Vector2.up, Vector2.down }, isDestroynigFound);      

        if (isDestroynigFound && _isMatchFound)
        {
            _isMatchFound = false; 
            FillNulls();
        }

    }

    private void ClearMatch(List<TileView> matchingTiles)
    {
        for (int i = 0; i < matchingTiles.Count; i++)
        {
            matchingTiles[i].GetComponent<TileView>().Destroy();
        }
    }

    public IEnumerator FindNullTiles()
    {
        _findNullsMethodExecutedAtOnceCounter++;

        for (int x = 0; x < _xSize; x++)
        {
            for (int y = 0; y < _ySize; y++)
            {
                if (_tileViews[x, y].Tile == null)
                {
                    yield return StartCoroutine(ShiftTilesDown(x, y));
                }
            }
        }

        for (int x = 0; x < _xSize; x++)
        {
            for (int y = 0; y < _ySize; y++)
            {
                FindAllMatchesForTile(_tileViews[x, y]);
            }
        }

        _findNullsMethodExecutedAtOnceCounter--;

        if (_findNullsMethodExecutedAtOnceCounter == 0)
        {
            TurnsCounter.Instance.Turns++;
            CheckMovingTiles();
            
            CheckAllTileCurses();
        }

    }

    private IEnumerator ShiftTilesDown(int x, int yStart, float shiftDelay = .055f)
    {
        State = BoardStates.Shifting;
        List<TileView> tiles = new List<TileView>();
        int nullCount = 0;
        bool isCounterStopped = false;

        for (int y = yStart; y < _ySize; y++)
        {
            TileView tile = _tileViews[x, y].GetComponent<TileView>();
            if (tile.Tile == null && !isCounterStopped)
            {
                nullCount++;
            }
            else
            {
                isCounterStopped = true;
            }

            tiles.Add(tile);
        }

        for (int i = 0; i < nullCount; i++)
        {
            yield return new WaitForSeconds(shiftDelay);
            for (int k = 0; k < tiles.Count - 1; k++)
            {
                tiles[k].Tile = tiles[k + 1].Tile;
                tiles[k + 1].Tile = GetNewTile(x, _ySize - 1);

                tiles[k].Curses = tiles[k + 1].Curses;
                tiles[k + 1].Curses = new TileCurses();
            }
            if (tiles.Count == 1)
            {
                tiles[0].Tile = GetNewTile(x, _ySize - 1);
                tiles[0].Curses = new TileCurses();
            }
        }
        State = BoardStates.Game;
    }

    private Tile GetNewTile(int x, int y)
    {
        List<Tile> possibleTiles = new List<Tile>();
        possibleTiles.AddRange(PossibleTiles);

        if (x > 0)
        {
            possibleTiles.Remove(_tileViews[x - 1, y].Tile);
        }
        if (x < _xSize - 1)
        {
            possibleTiles.Remove(_tileViews[x + 1, y].Tile);
        }
        if (y > 0)
        {
            possibleTiles.Remove(_tileViews[x, y - 1].Tile);
        }

        return possibleTiles[Random.Range(0, possibleTiles.Count)];
    }


    public void SetBonusAndWaitForTile(IBonus bonus, AudioClip sound)
    {
        TileView.DeselectAll();
        _bonus = bonus;
        _bonusSound = sound;
        State = BoardStates.BonusAwaiting;
    }

    public void CancelBonus()
    {
        State = BoardStates.Game;
        _bonus = null;
    }

    public void ExecuteBonus(TileView tile)
    {
        _bonus.Execute(tile, _tileViews);
        AudioPlayer.PlaySound(_bonusSound);
        CancelBonus();
        FillNulls();
        TurnsCounter.Instance.Turns++;
        OnBonusEnd?.Invoke(true);
    }

    public void CheckMovingTiles()
    {
        for (int x = 0; x < _xSize; x++)
        {
            for (int y = 0; y < _ySize; y++)
            {
                if (!_tileViews[x, y].IsAlreadyMoved &&
                    _tileViews[x, y].MovingDirection() != Vector2.zero)
                {
                    MoveTile(_tileViews[x, y], _tileViews[x, y].MovingDirection());
                }
            }
        }

        for (int x = 0; x < _xSize; x++)
        {
            for (int y = 0; y < _ySize; y++)
            {
                _tileViews[x, y].IsAlreadyMoved = false;
            }
        }

        IsThereMatches(true);
    }

    public void MoveTile(TileView tile, Vector2 direction)
    {
        tile.gameObject.SetActive(false);
        RaycastHit2D hit = Physics2D.Raycast(tile.transform.position, direction);
        tile.gameObject.SetActive(true);

        TileView tempTileView;
        if (hit.collider == null ||
            !hit.collider.TryGetComponent(out tempTileView))
        {
            tile.Curses.ReverseMovingDirection();
        }
        else
        {
            Swap(tile, tempTileView, false);
            tempTileView.IsAlreadyMoved = true;
        }
    }
}