using DG.Tweening;
using HolagoGames;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TileController : MonoBehaviour
{   
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private TextMeshProUGUI numberText;
    [SerializeField] private Color[] tileColors;

    private int tileValue;
    private int moveCounter;

    private void Awake()
    {
        inputHandler = FindObjectOfType<InputHandler>(true);
    }

    private void OnEnable()
    {
        Holago.SystemContainer.EventSystem.OnTileMoveStarted.Register(OnTileMoveStarted);
        Holago.SystemContainer.EventSystem.OnTileMoveFinished.Register(OnTileMoveFinished);
        Holago.SystemContainer.EventSystem.OnMergeCompleted.Register(OnMergeCompleted);
    }

    private void OnDisable()
    {
        DOTween.Kill(transform);
        DOTween.Kill(GetInstanceID());

        Holago.SystemContainer.EventSystem.OnTileMoveStarted.UnRegister(OnTileMoveStarted);
        Holago.SystemContainer.EventSystem.OnTileMoveFinished.UnRegister(OnTileMoveFinished);
        Holago.SystemContainer.EventSystem.OnMergeCompleted.UnRegister(OnMergeCompleted);
    }

    public void SetTile(int number)
    {
        tileValue = number;

        SetNumber(number);
        SetColor(number);
    }

    public int GetTileValue()
    {
        return tileValue;
    }

    private void SetNumber(int number)
    {
        numberText.text = number.ToString();
    }

    private void SetColor(int number)
    {
        int colorIndex = Mathf.RoundToInt(Mathf.Log(number, 2)) - 1;

        if (colorIndex >= 0 && colorIndex < tileColors.Length)
        {
            Image tileImage = GetComponent<Image>();
            tileImage.color = tileColors[colorIndex];
        }
    }

    public void ExplodeTile()
    {
        //ExplodeAnimation();
        Destroy(gameObject);
    }

    public void MoveTile(Vector3 target)
    {
        DOTween.Kill(transform);

        transform.DOMove(target, .2f).SetEase(Ease.InSine)
            .OnStart(() => 
            {
                Holago.SystemContainer.EventSystem.OnTileMoveStarted.Invoke();
            })
            .OnComplete(() =>
            {
                Holago.SystemContainer.EventSystem.OnTileMoveFinished.Invoke();

                if (moveCounter > 0) return;

                Holago.SystemContainer.EventSystem.OnMovesCompleted.Invoke();
            });
    }

    public void MergeWith(TileController otherTile)
    {
        int newNumber = int.Parse(numberText.text) * 2;

        SetTile(newNumber);
        PopAnimation();
        Destroy(otherTile.gameObject, 0.01f);
    }

    private void PopAnimation()
    {
        DOTween.Sequence()
            .SetId(GetInstanceID())
            .Append(transform.DOScale(Vector3.one * 1.1f, 0.05f))
            .Append(transform.DOScale(Vector3.one, 0.05f))
            .OnComplete(() => 
            { 
                Holago.SystemContainer.EventSystem.OnMergeCompleted.Invoke();
            });
    }

    private void OnTileMoveStarted()
    {
        moveCounter = 0;
        moveCounter++;
        inputHandler.IsInputEnabled = false;
    }

    private void OnTileMoveFinished()
    {
        moveCounter--;
        inputHandler.IsInputEnabled = true;
        Holago.SystemContainer.EventSystem.OnMergeStarted.Invoke();
    }

    private void OnMergeCompleted()
    {
        moveCounter = 0;
        Holago.SystemContainer.EventSystem.OnMovesCompleted.Invoke();
    }

}