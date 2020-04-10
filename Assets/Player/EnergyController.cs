using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyController : MonoBehaviour
{
    [SerializeField] public int level { get; private set; } = 0;
    [Header("Required Exp")]
    [SerializeField] private List<int> levels = new List<int>(new int[6] {0, 1, 10, 100, 1000, 10000});

    private int currentExperience = 0;
    private int maxLevel= 5;

    public void addExperience(int experience)
    {
        Debug.Log("Adding experience");
        if (level == maxLevel) return;
        currentExperience += experience;
        for(int i = level + 1; i <= maxLevel; i++)
        {
            if (currentExperience - levels[i] >= 0)
            {
                level++;
                currentExperience -= levels[i];
                if (level == maxLevel) currentExperience = 0;
                continue;
            }
            else break;
        }
    }

    public void resetLevel()
    {
        Debug.Log("Reseting level");
        level = 0;
    }
}
