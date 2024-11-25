using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameProgressManager : MonoBehaviour
{
    public TMP_Text highestStageText; // Reference to the UI Text for "Highest Stage"
    private const string HighestStageKey = "HighestStage";

    private void Start()
    {
        // Load and display the highest stage on the menu
        int highestStage = PlayerPrefs.GetInt(HighestStageKey, 0); // Default is 0
        UpdateHighestStageText(highestStage);
    }

    public void UpdateHighestStage(int newStage)
    {
        // Get the current saved highest stage
        int highestStage = PlayerPrefs.GetInt(HighestStageKey, 0);

        // Update the highest stage if the new stage is greater
        if (newStage > highestStage)
        {
            PlayerPrefs.SetInt(HighestStageKey, newStage);
            PlayerPrefs.Save();
            UpdateHighestStageText(newStage);
        }
    }

    private void UpdateHighestStageText(int highestStage)
    {
        if (highestStageText != null)
        {
            highestStageText.text = $"Highest Stage: {highestStage}";
        }
    }

    public void ResetProgress()
    {
        // Clear the saved highest stage
        PlayerPrefs.DeleteKey(HighestStageKey);
        PlayerPrefs.Save();

        // Update the displayed text
        UpdateHighestStageText(0);
    }
}
