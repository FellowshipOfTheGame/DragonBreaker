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

    private bool _facingRight = false;
    private bool _isAttacking = false;
    SFX _sfx;

    private void Awake() {
        _sfx = GetComponent<SFX>();
    } 

    public void Attack(Vector2 movementInput, bool facingRight)
    {
        // Don't attack if already in animation
        if (_isAttacking) return;

        const float minimum_input = 0.4f;
        bool isSideAttack = Math.Abs(movementInput.y) <= minimum_input || Math.Abs(movementInput.x) > minimum_input;

        //Debug.Log($"movementInput {movementInput}, minimum_input {minimum_input}");
        animator.SetFloat("Vertical attack", movementInput.y);
        animator.SetBool("Side attack", isSideAttack);
        animator.SetTrigger("Attack");
        _facingRight = facingRight;
        _isAttacking = true;

        if (Mathf.Abs(movementInput.y) >= minimum_input)
        {
            if (isSideAttack)
            {
                if (movementInput.y > 0)
                    Invoke("SideUpAttack", attackDelay);
                else if (movementInput.y < 0)
                    Invoke("SideDownAttack", attackDelay);
            }
            else
            {
                if (movementInput.y > 0)
                    Invoke("UpAttack", attackDelay);
                else if (movementInput.y < 0)
                    Invoke("DownAttack", attackDelay);
            }
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

        // Done attacking
        _isAttacking = false;
    }

    private void PerformAttack(Collider2D[] collisions)
    {
        Collider2D closestPlayer = null;
        Action<int> callbackFunction = null;

        _sfx.Play("attack");

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

        // Done attacking
        _isAttacking = false;
    }

    private void SideAttack()
    {
        //PerfomAttack(attackPoint.transform.position);
        Vector2 center = new Vector2((attackPoint.position.x + transform.position.x)/2, attackPoint.position.y);
        Vector2 size = new Vector2(Vector2.Distance(attackPoint.position, transform.position), thicness);
        var collisions = Physics2D.OverlapCapsuleAll(center, size, CapsuleDirection2D.Horizontal, 0, _attack_layer);
        PerformAttack(collisions);
        DrawCapsule(center, size, CapsuleDirection2D.Horizontal, 0);
        //_sfx.Play("attack");
    }

    private void SideUpAttack()
    {
        //PerfomAttack(attackPoint.transform.position);
        Vector2 center = new Vector2((attackPoint.position.x + transform.position.x) / 2, attackPoint.position.y) + new Vector2(0, Vector2.Distance(attackPoint.position, transform.position)/2);
        Vector2 size = new Vector2(Vector2.Distance(attackPoint.position, transform.position), thicness);
        float angle = _facingRight ? -45 : 45;
        var collisions = Physics2D.OverlapCapsuleAll(center, size, CapsuleDirection2D.Horizontal, angle, _attack_layer);
        PerformAttack(collisions);
        DrawCapsule(center, size, CapsuleDirection2D.Horizontal, angle);
        //_sfx.Play("attack");
    }

    private void SideDownAttack()
    {
        //PerfomAttack(attackPoint.transform.position);
        Vector2 center = new Vector2((attackPoint.position.x + transform.position.x) / 2, attackPoint.position.y) - new Vector2(0, Vector2.Distance(attackPoint.position, transform.position) / 1.8f);
        Vector2 size = new Vector2(Vector2.Distance(attackPoint.position, transform.position), thicness);
        float angle = _facingRight ? 45 : -45;
        var collisions = Physics2D.OverlapCapsuleAll(center, size, CapsuleDirection2D.Horizontal, angle, _attack_layer);
        DrawCapsule(center, size, CapsuleDirection2D.Horizontal, angle);
        PerformAttack(collisions);
        //_sfx.Play("attack");
    }

    private void UpAttack()
    {
        //PerformAttack(new Vector2(transform.position.x - 0.1f, Math.Abs(attackPoint.position.x)));
        Vector2 center = transform.position + new Vector3(0, Vector2.Distance(attackPoint.position, transform.position)/2);
        Vector2 size = new Vector2(thicness, Vector2.Distance(attackPoint.position, transform.position));
        var collisions = Physics2D.OverlapCapsuleAll(center, size, CapsuleDirection2D.Vertical, 0, _attack_layer);
        DrawCapsule(center, size, CapsuleDirection2D.Vertical, 0);
        PerformAttack(collisions);
        //_sfx.Play("attack");
    }

    private void DownAttack()
    {
        //PerformAttack(new Vector2(transform.position.x, transform.position.y) + Vector2.down * attackPoint.position.x);
        Vector2 center = transform.position - new Vector3(0, Vector2.Distance(attackPoint.position, transform.position)/1.8f);
        Vector2 size = new Vector2(thicness, Vector2.Distance(attackPoint.position, transform.position));
        var collisions = Physics2D.OverlapCapsuleAll(center, size, CapsuleDirection2D.Vertical, 0, _attack_layer);
        DrawCapsule(center, size, CapsuleDirection2D.Vertical, 0);
        PerformAttack(collisions);
        //_sfx.Play("attack");
    }

    //For debug purposes
#if true
    private CapsuleCollider2D coll = null;
    private void DrawCapsule(Vector2 center, Vector2 size, CapsuleDirection2D direction, float angle = 0)
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
        coll.transform.localRotation = Quaternion.identity;
        coll.transform.Rotate(Vector3.forward * angle);
        coll.isTrigger = true;
        //Debug.Break();
    }
#endif
    
}
