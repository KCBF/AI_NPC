using UnityEngine;
using TMPro;

public class MicrophoneDisplayUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI microphoneText;
    [SerializeField] private GameObject displayPanel;
    [SerializeField] private float displayDuration = 3f;

    private float displayTimer;
    private bool isDisplaying;

    private void Start()
    {
        if (displayPanel != null)
        {
            displayPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (isDisplaying)
        {
            displayTimer -= Time.deltaTime;
            if (displayTimer <= 0)
            {
                HideDisplay();
            }
        }
    }

    public void UpdateMicrophoneText(string microphoneName)
    {
        if (microphoneText != null)
        {
            microphoneText.text = $"Current Microphone: {microphoneName}";
        }
    }

    public void ShowDisplay()
    {
        if (displayPanel != null)
        {
            displayPanel.SetActive(true);
            displayTimer = displayDuration;
            isDisplaying = true;
        }
    }

    public void HideDisplay()
    {
        if (displayPanel != null)
        {
            displayPanel.SetActive(false);
            isDisplaying = false;
        }
    }
}