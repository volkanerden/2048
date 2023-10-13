using HolagoGames;
using UnityEngine;

public class CellController : MonoBehaviour
{
    public TileController[] tileArray { get; private set; }
    public bool IsOccupied { get; private set; }
    public bool IsFull { get; private set; }
    public bool IsActive { get; set; }
    public bool isExplodable;
    
    private void OnEnable()
    {
        Holago.SystemContainer.EventSystem.OnMergeStarted.Register(OnMergeStarted);
    }
    private void OnDisable()
    {
        Holago.SystemContainer.EventSystem.OnMergeStarted.UnRegister(OnMergeStarted);
    }

    public void Initialize()
    {
        tileArray = new TileController[2];
        IsOccupied = false;
        IsFull = false;
    }

    public void PopulateCell(TileController tile)
    {
        if (tileArray[0] == null)
        {
            tileArray[0] = tile;
            IsOccupied = true;
        }

        else if (tileArray[0] != null && tileArray[1] == null)
        {
            tileArray[1] = tile;
            IsOccupied = true;
            IsFull = true;
        }
    }
    
    public void ClearCell()
    {
        for (int i = 0; i < tileArray.Length; i++)
        {
            tileArray[i] = null;
            IsOccupied = false;
        }
    }

    public void MergeTiles()
    {
        if (tileArray[0] != null && tileArray[1] != null)
        {
            if (CanMerge())
            {
                tileArray[0].MergeWith(tileArray[1]);
                tileArray[1] = null;
                
                IsFull = false;
            }
        }
    }

    public bool CanMerge()
    {
        return tileArray[0].GetTileValue() == tileArray[1].GetTileValue();
    }

    private void OnMergeStarted()
    {
        MergeTiles();
    }
}