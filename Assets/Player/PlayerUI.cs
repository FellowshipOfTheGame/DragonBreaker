using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Text textUI;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image expBar;

    private HealthSystem playerHealth;
    private EnergyController playerEnergy;

    /// <summary>
    /// Enable callbacks for player's UI
    /// </summary>
    /// <param name="player"></param>
    public void Setup(GameObject player)
    {
        playerHealth = player.GetComponent<HealthSystem>();
        playerEnergy = player.GetComponent<EnergyController>();

        playerHealth.onHealthChange += HealthChange;
        playerEnergy.onLevelChange += LevelChange;
        playerEnergy.onExperienceChange += ExperienceChange;
    }

    /// <summary>
    /// When player dies, stop listening for events
    /// </summary>
    public void Deactivate()
    {
        playerHealth.onHealthChange -= HealthChange;
        playerEnergy.onLevelChange -= LevelChange;
        playerEnergy.onExperienceChange -= ExperienceChange;
    }

    /// <summary>
    /// Change player health UI when player health's changes
    /// </summary>
    private void HealthChange(float healthPercent)
    {
        healthBar.fillAmount = healthPercent;
    }

    /*
     * update player level UI
     */
    private void LevelChange(int level)
    {
        textUI.text = level.ToString();
    }

    private void ExperienceChange(float expPercent)
    {
        expBar.fillAmount = expPercent;
    }

    // Free delegate
    private void OnDestroy()
    {
        playerHealth.onHealthChange -= HealthChange;
        playerEnergy.onLevelChange -= LevelChange;
        playerEnergy.onExperienceChange -= ExperienceChange;
    }
}
