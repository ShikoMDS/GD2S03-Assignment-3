using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIManager : MonoBehaviour
{
    public Image[] healthIcons; // Array of health icons in UI
    public Sprite fullHeart;    // Sprite for full health icon (e.g., heart)
    public Sprite emptyHeart;   // Sprite for empty health icon (e.g., broken heart)

    private int maxHealth;
    private int currentHealth;

    public void InitializeHealth(int maxHealth)
    {
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;

        // Update UI to reflect max health
        for (int i = 0; i < healthIcons.Length; i++)
        {
            if (i < maxHealth)
            {
                healthIcons[i].gameObject.SetActive(true);
            }
            else
            {
                healthIcons[i].gameObject.SetActive(false);
            }
        }

        UpdateHealthDisplay();
    }

    public void UpdateHealth(int currentHealth)
    {
        this.currentHealth = currentHealth;
        UpdateHealthDisplay();
    }

    private void UpdateHealthDisplay()
    {
        for (int i = 0; i < healthIcons.Length; i++)
        {
            if (i < currentHealth)
            {
                healthIcons[i].sprite = fullHeart;
            }
            else
            {
                healthIcons[i].sprite = emptyHeart;
            }
        }
    }
}
