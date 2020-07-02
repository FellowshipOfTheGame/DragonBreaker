using System;
using UnityEngine;

public class Breaker : MonoBehaviour
{
    [SerializeField] private EnergyController energyController;
    [SerializeField] private LayerMask _attack_layer;
    [SerializeField] private Animator animator;

    [Header("Attack Properties")]
    [SerializeField] public float range = 1f;

    public void Attack(Vector2 movementInput, bool facingRight)
    {
        const float minimum_input = 0.4f;
        animator.SetFloat("Vertical attack", movementInput.y);
        animator.SetBool("Side attack", Math.Abs(movementInput.y) <= minimum_input || Math.Abs(movementInput.x) > minimum_input);
        animator.SetTrigger("Attack");
        Debug.Log("Attacking");
        float x_offset = movementInput.x, y_offset = movementInput.y;
        if (movementInput == Vector2.zero)
        {
            if (!facingRight) x_offset = 1;
            else x_offset = -1;
        }

        Vector2 attack_position = new Vector2(x_offset + transform.position.x, y_offset + transform.position.y);

        //Check 4 directions
        if (movementInput.y > minimum_input)
            attack_position = transform.position + Vector3.up;
        else if (movementInput.y < -minimum_input)
            attack_position = transform.position + Vector3.down;
        else if (movementInput.x > minimum_input)
            attack_position = transform.position + Vector3.right;
        else if (movementInput.x < -minimum_input)
            attack_position = transform.position + Vector3.left;

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
}
