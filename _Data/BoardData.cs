using UnityEngine;

[CreateAssetMenu(fileName = "Board Data", menuName = "ScriptableObjects/Board Data")]
public class BoardData : ScriptableObject
{
    public int rows;
    public int columns;
    public bool[] cellData;
}