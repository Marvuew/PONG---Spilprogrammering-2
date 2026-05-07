using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour
{
    public int magnitude = 1;

    Vector2 direction;

    public Rigidbody2D rb;

    public GameObject OutZone1;
    public GameObject OutZone2;

    public GameObject Edge1;
    public GameObject Edge2;

    int x;

    Vector3 startBallPos;
    Vector2 left = new Vector2(-1, 0);
    Vector2 right = new Vector2(1, 0);
    Vector2[] initialDirections;

    public List<GameObject> players = new List<GameObject>();

    
    void Start()
    {
        initialDirections = new Vector2[2] { left, right };
        rb = GetComponent<Rigidbody2D>();
        startBallPos = transform.position;

        Push();
    }

    public void Push()
    {
        int randomIndex = Random.Range(0, initialDirections.Length);
        direction = initialDirections[randomIndex];
     
        rb.linearVelocity = direction * magnitude;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == OutZone1 || collision.gameObject == OutZone2)
        {
            if (collision.gameObject == OutZone2) ScoreManager.instance.OneScore();
            if (collision.gameObject == OutZone1) ScoreManager.instance.TwoScore();

            transform.position = startBallPos;
            GameManager.instance.SetUpScene();
            Push();
        }
        if (collision.gameObject == Edge1 || collision.gameObject == Edge2)
        {
            direction.y *= -1;
        }

        if (players.Contains(collision.gameObject))
        {
            CalculateAngle(collision);
            direction.x *= -1;
        }

        rb.linearVelocity = direction * magnitude;
    }

    public void CalculateAngle(Collision2D collision)
    {
        Vector2 collisionPoint = collision.GetContact(0).point;
        Vector2 paddleCenter = collision.collider.bounds.center;
        float offset = collisionPoint.y - paddleCenter.y;
        float maxOffset = collision.collider.bounds.extents.y;
        float normalizedOffset = offset / maxOffset;
        float bounceAngle = normalizedOffset * 30f; // Max bounce angle of 30 degrees
        direction = Quaternion.Euler(0, 0, bounceAngle) * direction;
    }



}
