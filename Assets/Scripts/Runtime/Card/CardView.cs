using UnityEngine;
using UnityEngine.EventSystems;

public class CardView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Spell SpellPrefab;
    [SerializeField] private RectTransform handArea;
    [SerializeField] private RectTransform target;

    [SerializeField] private CanvasGroup canvasGroup;

    private RectTransform rectTransform;
    private Camera _mainCamera;
    private Spell _currentSpell;

    private void Awake()
    {
        _mainCamera = Camera.main;
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData pointerEventData)
    {
        if (pointerEventData.button != PointerEventData.InputButton.Left)
            return;

        _currentSpell = Instantiate(SpellPrefab);
    }
    public void OnDrag(PointerEventData pointerEventData)
    {
        if (pointerEventData.button != PointerEventData.InputButton.Left)
            return;

        bool isInsideHand = RectTransformUtility.RectangleContainsScreenPoint(
            handArea,
            pointerEventData.position,
            pointerEventData.pressEventCamera
        );

        if (isInsideHand)
        {
            canvasGroup.alpha = 1;
            _currentSpell.HidePreview();
        }
        else
        {
            canvasGroup.alpha = 0;
            _currentSpell.ShowPreview();
        }
    }
    public void OnEndDrag(PointerEventData pointerEventData)
    {
        if (pointerEventData.button != PointerEventData.InputButton.Left)
            return;
            
        bool isInsideHand = RectTransformUtility.RectangleContainsScreenPoint(
            handArea,
            pointerEventData.position,
            pointerEventData.pressEventCamera
        );

        if (isInsideHand)
        {
            rectTransform.anchoredPosition = target.anchoredPosition;
            Destroy(_currentSpell.gameObject);
        }
        else
        {
            _currentSpell.Execute();
            Destroy(_currentSpell.gameObject);

            Destroy(gameObject);
        }
    }
}