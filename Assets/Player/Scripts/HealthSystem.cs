using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour, IDamagable
{
    public event Action<float> onHealthChange;
    public event Action onDamageTaken;
    public event Action onDeath;
    SFX _sfx;
    public float HealthPercent => health / MaxHealth;
    public bool Invulnerable = false;

    [Header("Health Properties")]
    [SerializeField] private float MaxHealth = 5f;
    [SerializeField] private float health = 0f;
    [SerializeField] private float invulnerabilityTime = 0.5f;
     
    private void Awake()
    {
        health = MaxHealth;
        onDeath += die;
        _sfx = GetComponent<SFX>();
    }

    public void hit(float damage, Action<int> callback)
    {
        if (Invulnerable || damage == 0f)
            return;
        Debug.Log("Taking damage " + gameObject.name);
        
        //Taking damage
        health -= damage;
        _sfx.Play("damage");
        
        //Making invulnerable
        Invulnerable = true;
        Invoke("MakeVulnerable", invulnerabilityTime);

        //Check death
        if (health <= 0f)
            onDeath?.Invoke();

        //Invoke onDamage and onHealthChange events
        onDamageTaken?.Invoke();
        onHealthChange?.Invoke(HealthPercent);
        //Invoke callback
        callback?.Invoke(0);
    }

    private void die() => gameObject.SetActive(false);
    private void MakeVulnerable() => Invulnerable = false;
    private void OnDestroy() => onDeath -= die;
}
