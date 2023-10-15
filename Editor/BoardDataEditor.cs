using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BoardData))]
public class BoardDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        BoardData boardData = (BoardData)target;

        GUILayout.Label("Grid Size:");
        var prevRows = boardData.rows;
        var prevCols = boardData.columns;
        var curRows = EditorGUILayout.IntField("Rows", prevRows);
        var curCols = EditorGUILayout.IntField("Columns", prevCols);

        bool isDirty = false;
        if (curRows != prevRows || curCols != prevCols)
        {
            boardData.rows = curRows;
            boardData.columns = curCols;
            var prevCellData = boardData.cellData;
            boardData.cellData = new bool[curRows * curCols];
            isDirty = true;

            for (int row = 0; row < Mathf.Min(prevRows, curRows); row++)
                for (int col = 0; col < Mathf.Min(prevCols, curCols); col++)
                {
                    boardData.cellData[row * curCols + col] = prevCellData[row * prevCols + col];
                }
        }

        GUILayout.Space(20);
    
        GUILayout.Label("Cell isActive Flags:");
        for (int row = 0; row < curRows; row++)
        {
            GUILayout.BeginHorizontal();
            for (int column = 0; column < curCols; column++)
            {
                var prevValue = boardData.cellData[row * curCols + column];
                var curValue = EditorGUILayout.Toggle(prevValue);

                if (curValue != prevValue)
                {
                    boardData.cellData[row * curCols + column] = curValue;
                    isDirty = true;
                }
            }
            GUILayout.EndHorizontal();
        }

        if (isDirty)
        {
            EditorUtility.SetDirty(boardData);
            AssetDatabase.SaveAssetIfDirty(boardData);
        }
    }
}