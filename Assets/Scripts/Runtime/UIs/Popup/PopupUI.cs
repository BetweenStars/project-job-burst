using UnityEngine;
using UnityEngine.UI;

public abstract class PopupUI : MonoBehaviour
{
    public static PopupUI currentPopup;
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        closeButton.onClick.AddListener(Close);
    }

    private void OnDestroy()
    {
        closeButton.onClick.RemoveListener(Close);
    }

    public void Open()
    {
        if (currentPopup != null && currentPopup == this) return;

        if (currentPopup != null && currentPopup != this)
        {
            currentPopup.Close();
        }

        gameObject.SetActive(true);
        currentPopup = this;
    }
    public void Close()
    {
        gameObject.SetActive(false);
        currentPopup = null;
    }
}
