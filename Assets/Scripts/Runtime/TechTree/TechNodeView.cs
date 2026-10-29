using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TechNodeView : MonoBehaviour
{
    [SerializeField] private TechTreePopupView treeView;
    [SerializeField] private TechNode node;

    [SerializeField] private GameObject selectedFrame;

    public TechNode Node => node;

    private Button _nodeButton;

    private void Awake()
    {
        if (treeView == null) treeView = GetComponentInParent<TechTreePopupView>();
        
        _nodeButton = GetComponent<Button>();
        _nodeButton.onClick.AddListener(OnClick);
    }

    private void OnDestroy()
    {
        _nodeButton.onClick.RemoveListener(OnClick);
    }

    public void OnClick() => treeView.OnNodeClick(this);

    public void SetSelected(bool selected)
    {
        selectedFrame.SetActive(selected);
    }
}