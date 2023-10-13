using UnityEngine;

public class TileFactory : MonoBehaviour
{
    [SerializeField] private TileController tilePrefab;

    public TileController GenerateDefaultTile(Vector3 position)
    {
        TileController newTile = Instantiate(tilePrefab, position, Quaternion.identity, transform);
        return newTile;
    }
}