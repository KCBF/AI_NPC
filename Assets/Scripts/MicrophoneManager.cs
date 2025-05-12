using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Linq;

public class MicrophoneManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI microphoneDisplayText;
    [SerializeField] private InputActionProperty cycleMicrophoneAction; // For Oculus Touch A button
    [SerializeField] private MicrophoneDisplayUI displayUI; // Reference to the UI component

    private List<string> microphoneDevices = new List<string>();
    private int currentMicrophoneIndex = 0;
    private bool isListening = false;

    private void Awake()
    {
        // Initialize input action for Oculus Touch A button
        cycleMicrophoneAction.action.Enable();
        cycleMicrophoneAction.action.performed += OnCycleMicrophonePerformed;
    }

    private void Start()
    {
        RefreshMicrophoneList();
        UpdateMicrophoneDisplay();
    }

    private void OnDestroy()
    {
        if (cycleMicrophoneAction.action != null)
        {
            cycleMicrophoneAction.action.performed -= OnCycleMicrophonePerformed;
            cycleMicrophoneAction.action.Disable();
        }
    }

    private void Update()
    {
        // Check for M key press on desktop
        if (Input.GetKeyDown(KeyCode.M))
        {
            CycleMicrophone();
        }
    }

    private void OnCycleMicrophonePerformed(InputAction.CallbackContext context)
    {
        CycleMicrophone();
    }

    private void RefreshMicrophoneList()
    {
        microphoneDevices.Clear();
        foreach (string device in Microphone.devices)
        {
            microphoneDevices.Add(device);
        }

        // If no microphones found, add a default message
        if (microphoneDevices.Count == 0)
        {
            microphoneDevices.Add("No Microphone Found");
        }
    }

    private void CycleMicrophone()
    {
        if (microphoneDevices.Count == 0)
        {
            RefreshMicrophoneList();
        }

        currentMicrophoneIndex = (currentMicrophoneIndex + 1) % microphoneDevices.Count;
        UpdateMicrophoneDisplay();

        // Update UI display
        if (displayUI != null)
        {
            displayUI.UpdateMicrophoneText(microphoneDevices[currentMicrophoneIndex]);
            displayUI.ShowDisplay();
        }

        // If we're currently listening, restart with the new microphone
        if (isListening)
        {
            StopListening();
            StartListening();
        }
    }

    private void UpdateMicrophoneDisplay()
    {
        if (microphoneDisplayText != null)
        {
            string displayText = $"Microphone: {microphoneDevices[currentMicrophoneIndex]}";
            microphoneDisplayText.text = displayText;
        }
    }

    public void StartListening()
    {
        if (microphoneDevices.Count > 0 && !string.IsNullOrEmpty(microphoneDevices[currentMicrophoneIndex]))
        {
            isListening = true;
            // Add your microphone start logic here
            // For example, if you're using Meta Voice SDK, you would update its microphone device
            Debug.Log($"Started listening with microphone: {microphoneDevices[currentMicrophoneIndex]}");
        }
    }

    public void StopListening()
    {
        isListening = false;
        // Add your microphone stop logic here
        Debug.Log("Stopped listening");
    }

    public string GetCurrentMicrophoneName()
    {
        return microphoneDevices.Count > 0 ? microphoneDevices[currentMicrophoneIndex] : "No Microphone";
    }
}