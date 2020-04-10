using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour, IDamagable
{
    [SerializeField] private int _experience_reward;
    [SerializeField] private int _required_level;

    public void hit(float energy, Action<int> callback)
    {
        //Test if enough Energy
        if (_required_level > energy) return;

        gameObject.SetActive(false);
        callback(_experience_reward);
    }
}
