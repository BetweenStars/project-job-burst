using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour
{
    [Header("Currency Views")]
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text populationText;
    [SerializeField] private TMP_Text EnemyCountText;

    [Header("Progression Views")]
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private Slider townHpSlider;
    [SerializeField] private TMP_Text townHpText;

    private void Update()
    {
        UpdateTimeText();
    }

    public void UpdateHpViews(int currentHp, int maxHp)
    {
        townHpSlider.value = (float)currentHp / maxHp;
        townHpText.text = $"{currentHp} / {maxHp}";
    }

    public void UpdateMoneyViews(int money)
    {
        moneyText.text = money.ToString("N0");
    }

    private void UpdateTimeText()
    {
        int totalSeconds = Mathf.FloorToInt(GameManager.Instance.elapsedTime);
        int min = totalSeconds / 60;
        int sec = totalSeconds % 60;
        timeText.text = $"{min:00}:{sec:00}";
    }
}
