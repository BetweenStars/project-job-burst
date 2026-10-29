using UnityEngine;

public class DebugSystem : MonoBehaviour
{
    [SerializeField] private float timeScale = 1f;

    private void Update()
    {
        Time.timeScale = timeScale;
    }
}