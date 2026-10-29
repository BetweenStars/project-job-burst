using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private const float MinSize = 8f;
    private const float MaxSize = 35f;

    [SerializeField] private Camera controlledCamera;
    [SerializeField] private Rect worldBounds = new Rect(-20f, -12f, 40f, 24f);
    [SerializeField, Min(0f)] private float zoomSpeed = 0.01f;

    private bool _isDragging;
    private Vector2 _previousMousePosition;

    private void Awake()
    {
        if (controlledCamera == null) controlledCamera = GetComponent<Camera>();
        if (controlledCamera == null) controlledCamera = Camera.main;

        if (controlledCamera == null || !controlledCamera.orthographic)
        {
            Debug.LogError("[CameraController]An orthographic camera is required.", this);
            enabled = false;
            return;
        }

        controlledCamera.orthographicSize = 15f;
        Vector3 position = controlledCamera.transform.position;
        position.x = 0f;
        position.y = 0f;
        controlledCamera.transform.position = position;
    }

    private void LateUpdate()
    {
        float maxSize = Mathf.Min(MaxSize,
            worldBounds.height * 0.5f,
            worldBounds.width / (2f * controlledCamera.aspect));

        // 최소 줌 크기의 화면조차 들어가지 않는 경계는 유효한 설정이 아니다.
        if (maxSize < MinSize)
        {
            Debug.LogError("[CameraController]World Bounds must fit the camera at size 3 with the current aspect ratio.", this);
            enabled = false;
            return;
        }

        controlledCamera.orthographicSize = Mathf.Clamp(controlledCamera.orthographicSize, MinSize, maxSize);

        Mouse mouse = Mouse.current;
        if (mouse != null)
        {
            Vector2 mousePosition = mouse.position.ReadValue();
            bool isOverUI = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
            bool isInsideCamera = controlledCamera.pixelRect.Contains(mousePosition);

            if (!isOverUI && isInsideCamera)
            {
                ApplyZoom(mousePosition, mouse.scroll.ReadValue().y, maxSize);
            }

            if (mouse.leftButton.wasPressedThisFrame && !isOverUI && isInsideCamera)
            {
                _isDragging = true;
                _previousMousePosition = mousePosition;
            }

            if (!mouse.leftButton.isPressed) _isDragging = false;

            if (_isDragging)
            {
                Vector3 previous = controlledCamera.ScreenToWorldPoint(_previousMousePosition);
                Vector3 current = controlledCamera.ScreenToWorldPoint(mousePosition);
                controlledCamera.transform.position += previous - current;
                _previousMousePosition = mousePosition;
            }
        }
        else
        {
            _isDragging = false;
        }

        ClampPosition();
    }

    private void ApplyZoom(Vector2 mousePosition, float scroll, float maxSize)
    {
        if (Mathf.Approximately(scroll, 0f)) return;

        Vector3 before = controlledCamera.ScreenToWorldPoint(mousePosition);
        controlledCamera.orthographicSize = Mathf.Clamp(
            controlledCamera.orthographicSize - scroll * zoomSpeed, MinSize, maxSize);
        Vector3 after = controlledCamera.ScreenToWorldPoint(mousePosition);

        controlledCamera.transform.position += before - after;
    }

    private void ClampPosition()
    {
        float halfHeight = controlledCamera.orthographicSize;
        float halfWidth = halfHeight * controlledCamera.aspect;
        Vector3 position = controlledCamera.transform.position;
        position.x = Mathf.Clamp(position.x, worldBounds.xMin + halfWidth, worldBounds.xMax - halfWidth);
        position.y = Mathf.Clamp(position.y, worldBounds.yMin + halfHeight, worldBounds.yMax - halfHeight);
        controlledCamera.transform.position = position;
    }

    private void OnDisable()
    {
        _isDragging = false;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus) _isDragging = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(new Vector3(worldBounds.center.x, worldBounds.center.y, 0f),
            new Vector3(worldBounds.width, worldBounds.height, 0f));
    }
}
