using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;

public class Lobby : MonoBehaviour
{
    [SerializeField] Button create;
    [SerializeField] Button join;
    [SerializeField] GameObject code;
    [SerializeField] GameObject canvas;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        canvas.SetActive(false);
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
        canvas.SetActive(false);
    }
}
