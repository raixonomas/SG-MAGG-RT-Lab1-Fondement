using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [Header("Walk")]
    [SerializeField] private float speed = 7;
    [SerializeField] private float extraSpeedDecrease = 3;
    private Vector2 movementInput;
    private Vector3 direction;
    private Vector3 inputVelocity;
    private Vector3 extraVelocity;
    private Rigidbody2D rb;
    private CapsuleCollider2D capsCollider;


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

    public void SetInputValue(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    private void BuildHorizontalMovement()
    {
        direction = new Vector3(movementInput.x, 0f, movementInput.y).normalized;

        if (direction.magnitude >= 0.2f)
        {
            direction = direction.x * transform.right + direction.z * transform.forward;
            direction *= speed;
        }
        else
        {
            direction = Vector3.zero;
            inputVelocity.x = 0;
            inputVelocity.z = 0;
            extraVelocity.x = 0;
            extraVelocity.z = 0;
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

}

