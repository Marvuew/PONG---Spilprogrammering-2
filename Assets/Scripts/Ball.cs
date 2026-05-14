using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class Ball : NetworkBehaviour
{
    public int magnitude = 1;

    Vector2 direction;

    public Rigidbody2D rb;

    public GameObject OutZone1;
    public GameObject OutZone2;

    public GameObject Edge1;
    public GameObject Edge2;

    int x;

    public static event Action<int> OnGoalScored;

    Vector3 startBallPos;
    Vector2 left = new Vector2(-1, 0);
    Vector2 right = new Vector2(1, 0);
    Vector2[] initialDirections;

    public List<GameObject> players = new List<GameObject>();

    public bool isFrozen = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (!IsServer) return;

        SetUp();
        initialDirections = new Vector2[2] { left, right };
        startBallPos = transform.position;

        Push();
    }

    void FixedUpdate()
    {
        if (IsServer) return;

        // keep velocity consistent visually
        rb.linearVelocity = rb.linearVelocity.normalized * magnitude;
    }

    private void SetUp()
    {
        Debug.Log("Setup");
        OutZone1 = GameManager.instance.OutZone1;
        OutZone2 = GameManager.instance.OutZone2;
        Edge1 = GameManager.instance.Edge1;
        Edge2 = GameManager.instance.Edge2;
    }

    public void Push()
    {
        if (!IsServer) return;

        rb.linearVelocity = Vector2.zero;   // ?? IMPORTANT
        rb.angularVelocity = 0f;            // (just in case)

        int randomIndex = UnityEngine.Random.Range(0, initialDirections.Length);
        direction = initialDirections[randomIndex];

        rb.linearVelocity = direction.normalized * magnitude;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsServer) return;

        Debug.Log("Collided with: " + collision.gameObject.name);

        if (collision.gameObject == OutZone1 || collision.gameObject == OutZone2)
        {
            Debug.Log("Ball hit an OutZone: " + collision.gameObject.name);
            if (collision.gameObject == OutZone2)
            {
                OnGoalScored?.Invoke(1);
            }

            if (collision.gameObject == OutZone1)
            {
                OnGoalScored?.Invoke(2);
            }

            transform.position = startBallPos;

            Debug.Log("Ball reset to start position: " + transform.position);

            if (GameObject.FindGameObjectsWithTag("Player").Length != 0)
            {
                GameManager.instance.ResetPlayerPos();
            }

            Push();
            return;
        }

        // Edges (top/bottom)
        if (collision.gameObject == Edge1 || collision.gameObject == Edge2)
        {
            direction.y *= -1;
            GameManager.instance.ballHits++;
        }

        // Players
        if (collision.gameObject.CompareTag("Player"))
        {
            CalculateAngle(collision);      // THEN apply angle
            direction.x *= -1;              // flip first
            GameManager.instance.ballHits++;
        }

        // Apply velocity
        rb.linearVelocity = direction.normalized * magnitude;
    }

    [Rpc(SendTo.Server)]
    void ReportCollisionRpc(NetworkObjectReference victimRef)
    {
         
    }

    public void CalculateAngle(Collision2D collision)
    {

        Debug.Log("Calculation");
        Vector2 collisionPoint = collision.GetContact(0).point;
        Vector2 paddleCenter = collision.collider.bounds.center;
        float offset = collisionPoint.y - paddleCenter.y;
        float maxOffset = collision.collider.bounds.extents.y;
        float normalizedOffset = offset / maxOffset;
        float bounceAngle = normalizedOffset * 30f; // Max bounce angle of 30 degrees
        direction = new Vector2(direction.x, normalizedOffset).normalized;

        Debug.Log("Calculation 2" + direction);
    }
}
