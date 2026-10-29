using UnityEngine;
using UnityEngine.InputSystem;

public class Spell : MonoBehaviour
{
    [SerializeField] private GameObject preview;

    private void Awake()
    {
        HidePreview();
    }

    private void Update()
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

        transform.position = worldPos;
    }

    public void ShowPreview()
    {
        preview.SetActive(true);
    }
    public void HidePreview()
    {
        preview.SetActive(false);
    }
    public void Execute()
    {
       
    }
}