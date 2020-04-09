using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour, IDamagable
{
    [SerializeField] private int _required_energy;

    public void hit(float energy)
    {
        //Test if enough Energy
        if (_required_energy > energy) return;

        gameObject.SetActive(false);
    }
}
