using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TechTreePopupView : PopupUI
{
    private TechNodeView _currentNodeView;

    [Header("Node Info Views")]
    [SerializeField] private GameObject nodeInfoRoot;
    [SerializeField] private Button purchaseButton;
    [SerializeField] private TMP_Text nodeNameText;
    [SerializeField] private TMP_Text nodeDescriptionText;
    [SerializeField] private TMP_Text nodeCostText;

    public void OnPurchaseClick()
    {
        if (_currentNodeView == null) return;
    }

    public void OnNodeClick(TechNodeView nodeView)
    {
        bool deselect = _currentNodeView == nodeView;

        if (_currentNodeView != null)
            _currentNodeView.SetSelected(false);

        _currentNodeView = deselect ? null : nodeView;
        nodeInfoRoot.SetActive(_currentNodeView != null);

        if (_currentNodeView == null) return;

        _currentNodeView.SetSelected(true);
        ShowNodeInfo(_currentNodeView.Node);
    }

    public void ShowNodeInfo(TechNode node)
    {
        nodeNameText.text = node.DisplayName;
        nodeDescriptionText.text = node.Description;
        nodeCostText.text = node.Cost.ToString("N0");
    }
}