using UnityEngine;
using UnityEngine.InputSystem;

public class WorldPointer : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask interactableLayer;

    private IWorldInteractable currentTarget;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();

        Vector2 worldPos = mainCamera.ScreenToWorldPoint(screenPos);

        Collider2D hit =
            Physics2D.OverlapPoint(worldPos, interactableLayer);

        IWorldInteractable target = null;

        if (hit != null)
        {
            target = hit.GetComponent<IWorldInteractable>();
        }

        if (currentTarget != target)
        {
            currentTarget?.OnPointerExit();

            currentTarget = target;

            currentTarget?.OnPointerEnter();
        }

        if (currentTarget != null && Mouse.current.leftButton.wasReleasedThisFrame)
        {
            currentTarget?.OnClick();
        }
    }
}