using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] private float spawnRange;
    [SerializeField] private GameObject ballPrefab;
    private GameObject ball;

    private void Awake()
    {
        ball = Instantiate(ballPrefab);
        ball.SetActive(false);
    }

    private void Update()
    {
        if (!ball.activeInHierarchy)
        {
            SpawnBall();
        }
    }

    private void SpawnBall()
    {
        Vector2 vector2 = Vector2.zero;
        vector2.x = Random.Range(transform.position.x, transform.position.x + spawnRange);
        vector2.y = transform.position.y;
        ball.transform.position = vector2;
        ball.SetActive(true);
    }
}
