using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Knockbackable : MonoBehaviour
{
    [Header("Knockback properties")]
    [SerializeField] private float KnockbackForce = 100f;

    private Rigidbody2D _rigidbody = null;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void ApplyKnockback(Vector2 direction)
    {
        _rigidbody.AddForce(direction.normalized * KnockbackForce);
    }
}
