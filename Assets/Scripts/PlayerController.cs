using System.Globalization;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    public Rigidbody2D rb;

    public float moveSpeed;

    public bool isOwner;

    public bool isPlayer1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        NetworkObject networkObject = GetComponent<NetworkObject>();
        isOwner = networkObject.IsOwner;

        // Set position and save object by client id.
        if(networkObject.OwnerClientId == 0)
        {
            gameObject.transform.position = GameManager.instance.player1SpawnPos.transform.position;
            GameManager.instance.player1 = gameObject;
        }
        else
        {
            gameObject.transform.position = GameManager.instance.player2SpawnPos.transform.position;
            GameManager.instance.player2 = gameObject;
        }
    }

    private void FixedUpdate()
    {
        if (!isOwner)
        {
            return;
        }

        float dy = 0.0f;
        if (!isPlayer1)
        {
            if (InputSystem.GetDevice<Keyboard>().wKey.isPressed)
            {
                dy += 1;
            }

            if (InputSystem.GetDevice<Keyboard>().sKey.isPressed)
            {
                dy -= 1;
            }
        }
        else
        {
            if (InputSystem.GetDevice<Keyboard>().upArrowKey.isPressed)
            {
                dy += 1;
            }

            if (InputSystem.GetDevice<Keyboard>().downArrowKey.isPressed)
            {
                dy -= 1;
            }
        }
      

        rb.linearVelocityY = dy * moveSpeed * Time.fixedDeltaTime;
    }

}