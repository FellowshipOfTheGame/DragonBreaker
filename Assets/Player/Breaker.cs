using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breaker : MonoBehaviour
{

    [SerializeField] private LayerMask _attack_layer;

    [Header("Attack Properties")]
    [SerializeField] private float range = 1f;
    [SerializeField] public float power = 1f;

    public void Attack(Vector2 movementInput)
    {
        const float minimum_input = 0.3f;

        Vector2 attack_position;

        //Defaults attack to right
        if (movementInput != Vector2.zero)
            attack_position = new Vector2(movementInput.x + transform.position.x, movementInput.y + transform.position.y);
        else 
            attack_position = transform.position + Vector3.right;

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

            //Applying damage
            entity.GetComponent<IDamagable>()?.hit(power);
        }
    }
}
