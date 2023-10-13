using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    [SerializeField] TileFactory tileFactory;
    public void GenerateTile(CellController[,] grid)
    {
        CellController cell = GetRandomCell(grid);

        TileController tile = tileFactory.GenerateDefaultTile(cell.transform.position);
        int number = GenerateRandomNumber();
        tile.SetTile(number);
        cell.PopulateCell(tile);
    }

    private CellController GetRandomCell(CellController[,] grid)
    {
        List<CellController> emptyCells = GetEmptyCells(grid);

        int randomIndex = Random.Range(0, emptyCells.Count);
        return emptyCells[randomIndex];
    }

    private List<CellController> GetEmptyCells(CellController[,] grid)
    {
        List<CellController> emptyCells = new List<CellController>();

        foreach (CellController cell in grid)
        {
            if (!cell.IsOccupied && cell.IsActive)
            {
                emptyCells.Add(cell);
            }
        }

        return emptyCells;
    }

    private int GenerateRandomNumber()
    {
        int randomNumber = Random.Range(0, 10) < 9 ? 2 : 4;

        return randomNumber;
    }
}