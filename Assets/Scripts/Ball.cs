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


    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        /*x = Random.Range(-1, 1);
        y = Random.Range(-1, 1);

        while (x == 0) x = Random.Range(-1, 1);
        while (y == 0) y = Random.Range(-1, 1);

        direction = new Vector2(x, y);*/
        direction = new Vector2(-1, 0);
        rb.AddForce(direction * magnitude, ForceMode2D.Force);


        
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == OutZone1 || collision.gameObject == OutZone2)
        {
            SceneManager.LoadScene("Smilla");
        }
        else if (collision.gameObject == Edge1 || collision.gameObject == Edge2)
        {
            direction.y *= -1;
        }
        rb.AddForce(direction * magnitude, ForceMode2D.Force);
    }



}
