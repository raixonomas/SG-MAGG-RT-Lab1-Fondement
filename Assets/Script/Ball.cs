using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private string destroyMaskName;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == destroyMaskName)
        {
            gameObject.SetActive(false);
        }
    }

    public void HasBeenHit(Vector2 velocity)
    {
        Vector2 nextVelocity = rb.velocity;
        nextVelocity.y = 0;
        nextVelocity += velocity;
        rb.velocity = nextVelocity;
    }
}
