using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyController : MonoBehaviour
{
    [SerializeField] public int level { get; private set; } = 0;

    public System.Action<int> onLevelChange; 
    public System.Action<float> onExperienceChange; 

    [Header("Required Exp")]
    [SerializeField] private EXPTableSO xpTable = null;

    private int _currentExperience = 0;
    private int _maxLevel = 5;

    public void addExperience(int experience)
    {
        if (level == _maxLevel) return;
        _currentExperience += experience;
        onExperienceChange?.Invoke(_currentExperience / (float)xpTable.levels[level+1]);
        for(int i = level + 1; i <= _maxLevel; i++)
        { 
            if (_currentExperience - xpTable.levels[i] >= 0)
            {
                level++;
                _currentExperience -= xpTable.levels[i];
                onLevelChange?.Invoke(level);
                if (level == _maxLevel)
                {
                    _currentExperience = 0;
                    onExperienceChange?.Invoke(0);
                    return;
                }
                continue;
            }
            else break;
        }
        onExperienceChange?.Invoke(_currentExperience / (float)xpTable.levels[level+1]);
    }

    public void resetLevel()
    {
        level = 0;
        _currentExperience = 0;
        onLevelChange?.Invoke(level);
        onExperienceChange?.Invoke(_currentExperience / (float)xpTable.levels[level + 1]);
    }
}
