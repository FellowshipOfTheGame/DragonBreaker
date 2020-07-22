using System;
using System.Collections.Generic;
using UnityEngine;

public class Breaker : MonoBehaviour
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private EnergyController energyController;
    [SerializeField] private LayerMask _attack_layer;
    [SerializeField] private Animator animator;
    [SerializeField] private float attackDelay = 1 / 12;

    [Header("Attack Properties")]
    [SerializeField] public float range = 1f;

    public void Attack(Vector2 movementInput, bool facingRight)
    {
        const float minimum_input = 0.4f;

        animator.SetFloat("Vertical attack", movementInput.y);
        animator.SetBool("Side attack", Math.Abs(movementInput.y) <= minimum_input || Math.Abs(movementInput.x) > minimum_input);
        animator.SetTrigger("Attack");

        if (Mathf.Abs(movementInput.y) >= minimum_input)
        {
            if (movementInput.y > 0)
                Invoke("UpAttack", attackDelay);
            else
                Invoke("DownAttack", attackDelay);
        }
        else
            Invoke("SideAttack", attackDelay);

        return;

        
    }

    private void PerfomAttack(Vector2 attack_position)
    {
        //Checking collisions on attack area
        Collider2D[] collisions = Physics2D.OverlapAreaAll(transform.position, attack_position, _attack_layer);

        Collider2D closestPlayer = null;
        Action<int> callbackFunction = null;

        foreach (Collider2D entity in collisions)
        {

            if (entity.tag == "Object")
            {
                callbackFunction = experience => energyController.addExperience(experience);
                entity.GetComponent<IDamagable>()?.hit(energyController.level, callbackFunction);
            }
            else if (entity.tag == "Player" && entity.gameObject != gameObject && (!closestPlayer || (closestPlayer.transform.position - transform.position).sqrMagnitude > (entity.transform.position - transform.position).sqrMagnitude))
                closestPlayer = entity;
        }

        
        if (closestPlayer)
        {
            callbackFunction = _ => energyController.resetLevel();
            closestPlayer.GetComponent<IDamagable>()?.hit(energyController.level, callbackFunction);
        }
    }

    private void SideAttack()
    {
        PerfomAttack(attackPoint.transform.position);
    }

    private void UpAttack()
    {
        PerfomAttack(new Vector2(transform.position.x - 0.1f, Math.Abs(attackPoint.transform.position.x)));
    }
    private void DownAttack()
    {
        PerfomAttack(new Vector2(transform.position.x, transform.position.y) + Vector2.down * attackPoint.position.x);
    }
}
