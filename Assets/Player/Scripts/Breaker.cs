using System;
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
        Collider2D[] collisions = Physics2D.OverlapCircleAll(attack_position, range, _attack_layer);

        //TEMPORARY FIX to player having 2 colliders
        GameObject lastE = null;
        foreach (Collider2D entity in collisions)
        {
            //If hitting same twice or itself, continue
            if (entity.gameObject == lastE || entity.gameObject == gameObject) continue;
            lastE = entity.gameObject;
            Action<int> callbackFunction;
            if (entity.gameObject.layer == 9) callbackFunction = _ => energyController.resetLevel();
            else callbackFunction = experience => energyController.addExperience(experience);

            entity.GetComponent<IDamagable>()?.hit(energyController.level, callbackFunction);
        }
    }

    private void SideAttack()
    {
        PerfomAttack(attackPoint.position);
    }

    private void UpAttack()
    {
        PerfomAttack(new Vector2(transform.position.x, transform.position.y) + Vector2.up * attackPoint.position.x);
    }
    private void DownAttack()
    {
        PerfomAttack(new Vector2(transform.position.x, transform.position.y) + Vector2.down * attackPoint.position.x);
    }
}
