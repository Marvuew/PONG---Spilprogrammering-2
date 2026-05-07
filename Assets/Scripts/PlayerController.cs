using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;

    public float moveSpeed;

    public bool isOwner;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    private void FixedUpdate()
    {
        if (!isOwner)
        {
            return;
        }

        float dy = 0.0f;

        if (InputSystem.GetDevice<Keyboard>().wKey.isPressed || InputSystem.GetDevice<Keyboard>().upArrowKey.isPressed)
        {
            dy += 1;
        }

        if (InputSystem.GetDevice<Keyboard>().sKey.isPressed || InputSystem.GetDevice<Keyboard>().downArrowKey.isPressed)
        {
            dy -= 1;
        }

        rb.linearVelocityY = dy * moveSpeed * Time.fixedDeltaTime;
    }

}