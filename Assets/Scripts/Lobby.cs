using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Lobby : MonoBehaviour
{
    public static Lobby Instance;

    [SerializeField] Button create;
    [SerializeField] Button join;
    [SerializeField] GameObject code;
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject disconnectCanvas;



    public void Awake()
    {
        if (Instance == null || Instance != this)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

    }

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        canvas.SetActive(false);
        GameManager.instance.SpawnScoreManager();
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
        canvas.SetActive(false);
    }

    public void ShutDownClient()
    {
        StartCoroutine(ClientDisconnectRoutine());
    }

    public void ShutDownHost()
    {
        StartCoroutine(HostShutdownRoutine());
    }

    IEnumerator ClientDisconnectRoutine()
    {
        disconnectCanvas.SetActive(true);

        yield return new WaitForSeconds(5f);

        NetworkManager.Singleton.Shutdown(); // client leaves

        disconnectCanvas.SetActive(false);
        canvas.SetActive(true);
    }

    IEnumerator HostShutdownRoutine()
    {
        Debug.Log("SSSSS");
        disconnectCanvas.SetActive(true);
        Debug.Log("ddddd");

        yield return new WaitForSeconds(5f);
        Debug.Log("Canvas active BEFORE shutdown: " + canvas.activeSelf);

        // Update UI FIRST
        disconnectCanvas.SetActive(false);
        canvas.SetActive(true);

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        Debug.Log("Before shutdown. IsListening: " + NetworkManager.Singleton.IsListening);

        NetworkManager.Singleton.Shutdown();

        Debug.Log("After shutdown. IsListening: " + NetworkManager.Singleton.IsListening);
    }

    void OnDisable()
    {
        Debug.Log("UIManager DISABLED");
    }

    void OnDestroy()
    {
        Debug.Log("UIManager DESTROYED");
    }

}
