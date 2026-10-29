using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance { get; private set; }

    private PopupUI _currentPopup;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void OpenPopup(PopupUI popup)
    {
        if (_currentPopup == null)
        {
            popup.Open();
            _currentPopup = popup;

            return;
        }

        if (_currentPopup == popup)
        {
            _currentPopup.Close();
            _currentPopup = null;
        }
        else
        {
            _currentPopup.Close();
            _currentPopup = popup;
            _currentPopup.Open();
        }
    }
}