using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breaker : MonoBehaviour
{
    [SerializeField] private Transform _attack_point = null;
    [SerializeField] private LayerMask _attack_layer;

    public float attackRange = 1f;
    public float attackPower = 1f;

    public void OnHit()
    {
        Debug.Log("attacking");
        Collider2D[] collisions = Physics2D.OverlapCircleAll(_attack_point.position, attackRange, _attack_layer);
        foreach (Collider2D entity in collisions)
        {
            entity.GetComponent<IDamagable>()?.hit(attackPower);
        }
    }

    //Draw Attack Range gizmo
    void OnDrawGizmosSelected()
    {
        if (_attack_point == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(_attack_point.position, attackRange);
    }
}
