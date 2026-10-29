using Unity.VisualScripting;
using UnityEngine;

public class PopupOpener : MonoBehaviour, IWorldInteractable
{
    [SerializeField] private PopupUI targetPopup;
    [SerializeField] private GameObject outline;

    public void OnPointerEnter()
    {
        outline.SetActive(true);
    }
    public void OnPointerExit()
    {
        outline.SetActive(false);
    }
    public void OnClick()
    {
        targetPopup.Open();
    }
}