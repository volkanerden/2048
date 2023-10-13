using Unity.Burst;
using UnityEditorInternal;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BoardManager boardManager;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private TileGenerator tileGenerator;

    private int rows;
    private int columns;

    private void Start()
    {
        rows = boardManager.boardData.rows;
        columns = boardManager.boardData.columns;
    }

    public bool IsGameOver()
    {
        if (!CanMove() && !CanMerge())
        {
            gameOver.SetActive(true);
            return true;
        }
        return false;
    }

    private bool CanMove()
    {
        
        CellController[,] grid = boardManager.Grid;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (!grid[i, j].IsOccupied && grid[i,j].IsActive)
                    return true;
            }
        }
        return false;
    }

    private bool CanMerge()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                TileController currentTile = boardManager.Grid[i, j].tileArray[0];
                if (IsMergeable(currentTile, i + 1, j) ||
                    IsMergeable(currentTile, i - 1, j) ||
                    IsMergeable(currentTile, i, j + 1) ||
                    IsMergeable(currentTile, i, j - 1))
                    return true;
            }
        }
        return false;
    }

    private bool IsMergeable(TileController tile, int row, int column)
    {
        if (row >= 0 && row < rows && column >= 0 && column < columns)
        {
            TileController otherTile = boardManager.Grid[row, column].tileArray[0];
            CellController cell = boardManager.Grid[row, column];

            if (otherTile != null && tile != null && cell.IsActive)
            {
                return tile.GetTileValue() == otherTile.GetTileValue();
            }   
        }
        return false;
    }

    private void ResetGame()
    {
        gameOver.SetActive(false);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                CellController cellController = boardManager.Grid[i, j];
                if (cellController.IsOccupied)
                {
                    TileController occupyingTile = cellController.tileArray[0];
                    Destroy(occupyingTile.gameObject);
                    cellController.ClearCell();
                }
            }
        }
        tileGenerator.GenerateTile(boardManager.Grid);
        tileGenerator.GenerateTile(boardManager.Grid);
    }

    public void OnRestartButtonClicked()
    {
        ResetGame();
    }
}