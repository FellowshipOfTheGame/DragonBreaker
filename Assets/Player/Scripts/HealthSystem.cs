using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour, IDamagable
{
    public event Action<float> onHealthChange;
    public event Action onDamageTaken;
    public event Action onDeath;

    [Header("Health Properties")]
    [SerializeField] private float MaxHealth = 1f;
    [SerializeField] private float health = 0f;
    public float HealthPercent => health / MaxHealth;
     
    private void Awake()
    {
        health = MaxHealth;
        onDeath += die;
    }

    private void die()
    {
        gameObject.SetActive(false);
    }

    public void hit(float damage, Action<int> callback)
    {
        //Taking damage
        health -= damage;

        //Check death
        if (health <= 0f)
            onDeath?.Invoke();

        //Invoke onDamage and onHealthChange events
        onDamageTaken?.Invoke();
        onHealthChange?.Invoke(HealthPercent);
        //Invoke callback
        callback?.Invoke(0);
    }

    private void OnDestroy()
    {
        onDeath -= die;
    }
}
