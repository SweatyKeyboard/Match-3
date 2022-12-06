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

    private TileView[,] _tileViews;
    private RectTransform rectTransform;
    private IBonus _bonus;

    private float _width;
    private float _height;
    private float _xScale;
    private float _yScale;

    private bool _isMatchFound;
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

    public void Swap(TileView tile1, TileView tile2)
    {
        Tile tempTile = tile1.Tile;
        tile1.Tile = tile2.Tile;
        tile2.Tile = tempTile;

        TileCurses tempCurses = tile1.Curses;
        tile1.Curses = tile2.Curses;
        tile2.Curses = tempCurses;

        if (!IsThereMatches())
        {
            tempTile = tile1.Tile;
            tile1.Tile = tile2.Tile;
            tile2.Tile = tempTile;

            tempCurses = tile1.Curses;
            tile1.Curses = tile2.Curses;
            tile2.Curses = tempCurses;
        }
        else
        {
            TurnsCounter.Instance.Turns++;
        }
    }

    private TileView GetAdjacent(TileView tileView, Vector2 direction)
    {
        tileView.gameObject.SetActive(false);
        RaycastHit2D hit = Physics2D.Raycast(tileView.transform.position, direction);
        tileView.gameObject.SetActive(true);

        if (hit.collider?.GetComponent<TileView>())
        {
            return hit.collider.GetComponent<TileView>();
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

    private bool IsThereMatches()
    {
        _isMatchFound = false;
        for (int x = 0; x < _xSize; x++)
        {
            for (int y = 0; y < _ySize; y++)
            {
                FindAllMatchesForTile(_tileViews[x, y], false);
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

        while (hit.collider != null && hit.collider?.GetComponent<TileView>().Tile == tileView.Tile)
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

                tileView.Tile.Mana += matchingTiles.Count - 2;
                ScoreCounter.Instance.Score += matchingTiles.Count - 2;
                Timer.Instance.SecondsLeft += matchingTiles.Count - 2;

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
        for (int x = 0; x < _xSize; x++)
        {
            for (int y = 0; y < _ySize; y++)
            {
                if (_tileViews[x, y].Tile == null)
                {
                    yield return StartCoroutine(ShiftTilesDown(x, y));
                    break;
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
    }

    private IEnumerator ShiftTilesDown(int x, int yStart, float shiftDelay = .055f)
    {
        State = BoardStates.Shifting;
        List<TileView> tiles = new List<TileView>();
        int nullCount = 0;

        for (int y = yStart; y < _ySize; y++)
        { 
            TileView tile = _tileViews[x, y].GetComponent<TileView>();
            if (tile.Tile == null)
            {
                nullCount++;
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

    public void SetBonusAndWaitForTile(IBonus bonus)
    {
        TileView.DeselectAll();
        _bonus = bonus;
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
        CancelBonus();
        FillNulls();
        OnBonusEnd?.Invoke(true);
        TurnsCounter.Instance.Turns++;
    }
}