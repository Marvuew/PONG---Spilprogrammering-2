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

    public ScoreManager sm;

    Vector3 startBallPos;
    Vector2 left = new Vector2(-1, 0);
    Vector2 right = new Vector2(1, 0);
    Vector2[] initialDirections;

    public List<GameObject> players = new List<GameObject>();

    public bool isFrozen = false;

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
        Debug.Log("Collided with: " + collision.gameObject.name);
        if (collision.gameObject == OutZone1 || collision.gameObject == OutZone2)
        {
            Debug.Log("Ball hit an OutZone: " + collision.gameObject.name);
            if (collision.gameObject == OutZone2) sm.OneScore();
            if (collision.gameObject == OutZone1) sm.TwoScore();

            transform.position = startBallPos;
            Debug.Log("Ball reset to start position: " + transform.position);
            if (GameObject.FindGameObjectsWithTag("Player").Length != 0)
            {
                GameManager.instance.ResetPlayerPos();
            }
            Push();
        }
        if (collision.gameObject == Edge1 || collision.gameObject == Edge2)
        {
            direction.y *= -1;
            GameManager.instance.ballHits++;
        }

        if (players.Contains(collision.gameObject))
        {
            CalculateAngle(collision);
            direction.x *= -1;
            GameManager.instance.ballHits++;
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
