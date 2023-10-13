using UnityEngine;
using HolagoGames;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private CellController cellPrefab;
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private GameManager gameManager;
    private TileGenerator tileGenerator;
    public BoardData boardData;

    private float cellSpacing = 30f;
    private bool tileGenerated;

    public CellController[,] Grid { get; private set; }

    private void OnEnable()
    {
        Holago.SystemContainer.EventSystem.OnMovesCompleted.Register(OnMovesCompleted);
    }
    private void OnDisable()
    {
        Holago.SystemContainer.EventSystem.OnMovesCompleted.UnRegister(OnMovesCompleted);
    }

    private void Awake()
    {
        tileGenerator = GetComponent<TileGenerator>();
    }

    private void Start()
    {
        GenerateGrid();

        for (int i = 0; i < 2; i++)
        {
            tileGenerator.GenerateTile(Grid);
        }
    }

    private void OnMovesCompleted()
    {
        gameManager.IsGameOver();
        inputHandler.IsInputEnabled = true;
        if (tileGenerated) return;

        tileGenerator.GenerateTile(Grid);
        tileGenerated = true;
    }

    private void GenerateGrid()
    {
        Vector2 cellSize = cellPrefab.GetComponent<RectTransform>().sizeDelta;

        float centerX = (boardData.columns - 1) / 2f;
        float centerY = (boardData.rows - 1) / 2f;

        Grid = new CellController[boardData.rows, boardData.columns];

        for (int i = 0; i < boardData.rows; i++)
        {
            for (int j = 0; j < boardData.columns; j++)
            {
                Vector3 cellPosition = new Vector2((j - centerX) * (cellSize.x + cellSpacing),
                    (centerY - i) * (cellSize.y + cellSpacing));

                CellController cell = Instantiate(cellPrefab, transform);
                cell.transform.localPosition = cellPosition;

                cell.Initialize();

                Grid[i, j] = cell;
                Grid[i, j].IsActive = boardData.cellData[i * boardData.rows + j];

                SetCellVisibility(cell);
            }
        }
    }

    private void SetCellVisibility(CellController cell)
    {
        if (cell.IsActive)
        {
            cell.gameObject.GetComponent<Image>().enabled = true;
        }
        else
        {
            cell.gameObject.GetComponent<Image>().enabled = false;
        }
    }

    public void MoveTiles(bool isHorizontal)
    {
        tileGenerated = false;
        MoveInDirection(isHorizontal);
    }

    private void MoveInDirection(bool isHorizontal)
    {
        int line = isHorizontal ? boardData.columns : boardData.rows;
        int start = isHorizontal ? inputHandler.startColumn : inputHandler.startRow;
        int step = isHorizontal ? inputHandler.stepX : inputHandler.stepY;

        for (int i = 0; i < line; i++)
        {
            for (int j = start; j >= 0 && j < line; j -= step)
            {
                CellController currentCell = isHorizontal ? Grid[i, j] : Grid[j, i];
                while (true)
                {
                    if (!currentCell.IsOccupied) break;

                    int nextGrid = j + step;
                    if (nextGrid < 0 || nextGrid >= line) break;

                    CellController nextCell = isHorizontal ? Grid[i, nextGrid] : Grid[nextGrid, i];

                    if (nextCell.IsOccupied && currentCell.IsOccupied &&
                        nextCell.tileArray[0].GetTileValue() != currentCell.tileArray[0].GetTileValue())
                        break;

                    TileController movingTile = currentCell.tileArray[0];
                    if (nextCell.IsFull || !nextCell.IsActive) break;

                    nextCell.PopulateCell(movingTile);
                    movingTile.MoveTile(nextCell.transform.position);
                    currentCell.ClearCell();

                    j = nextGrid;
                    currentCell = nextCell;
                }
            }
        }
    }
}