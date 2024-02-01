using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 7;
    [SerializeField] private float extraSpeedDecrease = 3;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    private Vector2 movementInput;
    private Vector3 direction;
    private Vector3 inputVelocity;
    private Vector3 extraVelocity;
    private Rigidbody2D rb;
    private CapsuleCollider2D capsCollider;
    private bool canInput = true;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        capsCollider = GetComponent<CapsuleCollider2D>();
    }

    private void FixedUpdate()
    {
        BuildHorizontalMovement();
        FollowCameraRotation();
        if (extraVelocity.magnitude > 0.2f)
        {
            float reduction = (1 - extraSpeedDecrease * Time.fixedDeltaTime / extraVelocity.magnitude);
            extraVelocity *= reduction;
        }
        else extraVelocity = Vector3.zero;
        rb.velocity = inputVelocity + extraVelocity;
    }

    public void SetMovementInputValue(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }
    public void SetAttackInputValue(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0.5f)
            Attack();
    }

    private void BuildHorizontalMovement()
    {
        if (!canInput) return;
        direction = new Vector3(movementInput.x, 0f, movementInput.y).normalized;

        if (direction.magnitude >= 0.2f)
        {
            direction = direction.x * transform.right + direction.z * transform.forward;
            direction *= speed;
            if (direction.x > 0)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
            }
        }
        else
        {
            StopVelocity();
        }
        inputVelocity.x = direction.x;
        inputVelocity.z = direction.z;

    }

    private void FollowCameraRotation()
    {
        Vector3 rotation = Camera.main.transform.rotation.eulerAngles;
        rotation.x = 0f;
        transform.rotation = Quaternion.Euler(rotation);
    }

    private void StopVelocity()
    {
        direction = Vector3.zero;
        inputVelocity.x = 0;
        inputVelocity.z = 0;
        extraVelocity.x = 0;
        extraVelocity.z = 0;
    }

    private IEnumerator StopAttackFor(float time)
    {
        canInput = false;
        yield return new WaitForSeconds(time);
        canInput = true;
    }

    private void Attack()
    {
        Debug.Log("HAYAAAA");
        animator.SetTrigger("Attack");

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);

        if (colliders != null)
        {
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject != gameObject)
                {
                    Ball ball = collider.gameObject.GetComponent<Ball>();
                    if (ball != null)
                    {
                        ball.HasBeenHit(rb.velocity);
                        StopVelocity();
                        StartCoroutine(StopAttackFor(0.2f));
                    }
                }
            }
        }
    }
}
