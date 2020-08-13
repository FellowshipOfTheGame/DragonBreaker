using System;
using System.Collections.Generic;
using UnityEngine;

public class Breaker : MonoBehaviour
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private EnergyController energyController;
    [SerializeField] private LayerMask _attack_layer;
    [SerializeField] private Animator animator;

    [Header("Attack Properties")]
    [SerializeField] public float thicness = 1f;
    [SerializeField] private float range = 1f;
    [SerializeField] private float attackDelay = 1 / 4f;
    
    SFX _sfx;

    private void Awake() {
        _sfx = GetComponent<SFX>();
    } 

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

    private void PerformAttack(Vector2 attack_position)
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
            callbackFunction = _ =>
            {
                energyController.resetLevel();
                closestPlayer.GetComponent<Knockbackable>()?.ApplyKnockback(closestPlayer.transform.position - transform.position);
            };
            closestPlayer.GetComponent<IDamagable>()?.hit(energyController.level, callbackFunction);
        }
    }

    private void PerformAttack(Collider2D[] collisions)
    {
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
            callbackFunction = _ =>
            {
                energyController.resetLevel();
                closestPlayer.GetComponent<Knockbackable>()?.ApplyKnockback(closestPlayer.transform.position - transform.position);
            };
            closestPlayer.GetComponent<IDamagable>()?.hit(energyController.level, callbackFunction);
        }
    }

    private void SideAttack()
    {
        //PerfomAttack(attackPoint.transform.position);
        Vector2 center = new Vector2((attackPoint.position.x + transform.position.x)/2, attackPoint.position.y);
        Vector2 size = new Vector2(Vector2.Distance(attackPoint.position, transform.position), thicness);
        var collisions = Physics2D.OverlapCapsuleAll(center, size, CapsuleDirection2D.Horizontal, 0, _attack_layer);
        PerformAttack(collisions);
        //DrawCapsule(center, size, CapsuleDirection2D.Horizontal);
        _sfx.Play("attack");
    }

    private void UpAttack()
    {
        //PerformAttack(new Vector2(transform.position.x - 0.1f, Math.Abs(attackPoint.position.x)));
        Vector2 center = transform.position + new Vector3(0, Vector2.Distance(attackPoint.position, transform.position)/2);
        Vector2 size = new Vector2(thicness, Vector2.Distance(attackPoint.position, transform.position));
        var collisions = Physics2D.OverlapCapsuleAll(center, size, CapsuleDirection2D.Vertical, 0, _attack_layer);
        PerformAttack(collisions);
        _sfx.Play("attack");
    }
    private void DownAttack()
    {
        //PerformAttack(new Vector2(transform.position.x, transform.position.y) + Vector2.down * attackPoint.position.x);
        Vector2 center = transform.position - new Vector3(0, Vector2.Distance(attackPoint.position, transform.position)/1.8f);
        Vector2 size = new Vector2(thicness, Vector2.Distance(attackPoint.position, transform.position));
        var collisions = Physics2D.OverlapCapsuleAll(center, size, CapsuleDirection2D.Vertical, 0, _attack_layer);
        PerformAttack(collisions);
        _sfx.Play("attack");
    }

    //For debug purposes
#if false
    private CapsuleCollider2D coll = null;
    private void DrawCapsule(Vector2 center, Vector2 size, CapsuleDirection2D direction)
    {
        if(coll == null)
        {
            var go = new GameObject($"Visualize Capsule {gameObject.name}");
            coll = go.AddComponent<CapsuleCollider2D>();
        }
        coll.transform.position = center;
        coll.size = size;
        coll.offset = Vector2.zero;
        coll.direction = direction;
        coll.isTrigger = true;
    }
#endif
    
}
