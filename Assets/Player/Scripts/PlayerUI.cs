using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Text textUI = null;
    [SerializeField] private Image healthBar = null;
    [SerializeField] private Image expBar = null;

    [SerializeField] private Sprite[] healthBarProgress = new Sprite[5];

    private HealthSystem _playerHealth;
    private EnergyController _playerEnergy;

    /// <summary>
    /// Enable callbacks for player's UI
    /// </summary>
    /// <param name="player"></param>
    public void Setup(GameObject player)
    {
        _playerHealth = player.GetComponent<HealthSystem>();
        _playerEnergy = player.GetComponent<EnergyController>();

        healthBar.sprite = healthBarProgress[0];

        _playerHealth.onHealthChange += HealthChange;
        _playerEnergy.onLevelChange += LevelChange;
        _playerEnergy.onExperienceChange += ExperienceChange;
    }

    /// <summary>
    /// When player dies, stop listening for events
    /// </summary>
    public void Deactivate()
    {
        _playerHealth.onHealthChange -= HealthChange;
        _playerEnergy.onLevelChange -= LevelChange;
        _playerEnergy.onExperienceChange -= ExperienceChange;
    }

    /// <summary>
    /// Change player health UI when player health's changes
    /// </summary>
    private void HealthChange(float healthPercent)
    {
        //healthBar.fillAmount = healthPercent;
        if(healthPercent > 0.8f)
        {
            healthBar.sprite = healthBarProgress[0];
        }
        else if (healthPercent > 0.6f)
        {
            healthBar.sprite = healthBarProgress[1];
        }
        else if (healthPercent > 0.4f)
        {
            healthBar.sprite = healthBarProgress[2];
        }
        else if (healthPercent > 0.2f)
        {
            healthBar.sprite = healthBarProgress[3];
        }
        else if(healthPercent > 0.0f)
        {
            healthBar.sprite = healthBarProgress[4];
        }
        else
        {
            healthBar.sprite = null;
        }
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
        _playerHealth.onHealthChange -= HealthChange;
        _playerEnergy.onLevelChange -= LevelChange;
        _playerEnergy.onExperienceChange -= ExperienceChange;
    }
}
