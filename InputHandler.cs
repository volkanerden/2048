using UnityEngine;
using HolagoGames;
using UnityEngine.EventSystems;

public class InputHandler : MonoBehaviour
{
    private float RAYLENGTH = 100f;
    private bool isDragHandled = false;
    public bool IsInputEnabled { get; set; } = true;
    public bool IsHorizontal { get; private set; }
    public int startColumn { get; private set; }
    public int startRow { get; private set; }
    public int stepX { get; private set; }
    public int stepY { get; private set; }

    private void OnEnable()
    {
        Holago.SystemContainer.EventSystem.OnPointerDown.Register(OnPointerDown);
        Holago.SystemContainer.EventSystem.OnDrag.Register(OnDrag);
        Holago.SystemContainer.EventSystem.OnEndDrag.Register(OnEndDrag);
    }

    private void OnDisable()
    {
        Holago.SystemContainer.EventSystem.OnPointerDown.UnRegister(OnPointerDown);
        Holago.SystemContainer.EventSystem.OnDrag.UnRegister(OnDrag);
        Holago.SystemContainer.EventSystem.OnEndDrag.UnRegister(OnEndDrag);
    }

    private void OnPointerDown(PointerEventData data)
    {
        Ray ray = Holago.SystemContainer.CameraSystem.MainCamera.ScreenPointToRay(data.position);
        Debug.DrawRay(ray.origin, ray.direction * RAYLENGTH, Color.red, 2, true);
    }

    private void OnDrag(PointerEventData data)
    {
        if (isDragHandled ||
            !IsInputEnabled ||
            data.pointerCurrentRaycast.gameObject is not GameObject hitObject ||
            gameObject == null) 
            return;

        BoardManager boardManager = hitObject.GetComponentInChildren<BoardManager>();
        if (boardManager == null) return;

        Vector3 dragDirection = data.delta.normalized;

        BoardData boardData = boardManager.boardData;

        startColumn = dragDirection.x > 0 ? boardData.columns - 1 : 0;
        startRow = dragDirection.y > 0 ? 0 : boardData.rows - 1;

        stepX = dragDirection.x > 0 ? 1 : -1;
        stepY = dragDirection.y > 0 ? -1 : 1;

        IsHorizontal = Mathf.Abs(dragDirection.x) > Mathf.Abs(dragDirection.y);

        boardManager.MoveTiles(IsHorizontal);
        isDragHandled = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragHandled = false;
    }
}