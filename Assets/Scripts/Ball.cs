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
     
        rb.AddForce(direction * magnitude, ForceMode2D.Force);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == OutZone1 || collision.gameObject == OutZone2)
        {
            if (collision.gameObject == OutZone2) ScoreManager.OneScore();
            if (collision.gameObject == OutZone1) ScoreManager.TwoScore();

            transform.position = startBallPos;
            Push();
        }
        else if (collision.gameObject == Edge1 || collision.gameObject == Edge2)
        {
            direction.y *= -1;
        }
        rb.AddForce(direction * magnitude, ForceMode2D.Force);
    }



}
