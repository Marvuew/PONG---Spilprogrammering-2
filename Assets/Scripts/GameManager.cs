using System;
using System.Collections;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class GameManager : NetworkBehaviour
{
    public static GameManager instance;
    public GameObject PlayerPrefab;
    public ScoreManager scoreManager;

    public UnityEvent OnPlaySessionEnded = new UnityEvent();
    public UnityEvent OnPlaySessionStarted = new UnityEvent();
    public static event Action OnFinished;

    public void Awake()
    {
        if (instance == null || instance != this)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

    }
    public Transform player1SpawnPos;
    public Transform player2SpawnPos;

    [SerializeField] GameObject ball;

    public GameObject player1;
    public GameObject player2;
    GameObject scoreBoard;

    [Header("Game elements")]
    public GameObject OutZone1;
    public GameObject OutZone2;

    public GameObject Edge1;
    public GameObject Edge2;

    public GameObject logScreen;
    public GameObject lobbyCanvas;
    public GameObject disconnectCanvas;

    [Header("Log elements")]
    public Transform playLogContainer;
    public GameObject playLogTextPrefab;
    public GameObject Score;

    public int playSessionScorePlayer1;
    public int playSessionScorePlayer2;
    public int ballHits;

    public GameObject ScoreManager;
    private GameObject scoreInstance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void OnEnable()
    {
        OnPlaySessionEnded.AddListener(EndGameOfPong);
        OnPlaySessionStarted.AddListener(ResetScene);
    }

    public void OnDisable()
    {
        OnPlaySessionEnded.RemoveListener(EndGameOfPong);
        OnPlaySessionStarted.RemoveListener(ResetScene);
    }

    public void SpawnScoreManager()
    {
        if (!NetworkManager.Singleton.IsServer) return;

        scoreInstance = Instantiate(ScoreManager);
        scoreInstance.GetComponent<NetworkObject>().Spawn();
    }

    public void ResetScene()
    {
        ResetPlayerPos();
        scoreBoard = GameObject.FindWithTag("ScoreBoard");
        scoreBoard.GetComponent<ScoreManager>().Reset();
        ballHits = 0;
        ResumeGame();
    }

    public void ResetPlayerPos()
    {

        player1.transform.position = player1SpawnPos.position;
        player2.transform.position = player2SpawnPos.position;
        ResetPlayerPosClientRpc();
    }

    [ClientRpc]
    public void ResetPlayerPosClientRpc()
    {
        player1.transform.position = player1SpawnPos.position;
        player2.transform.position = player2SpawnPos.position;
    }

    public void EndGameOfPong()
    {
        OnFinished.Invoke();
        Debug.Log("Game Ended! Player 1 Score: " + playSessionScorePlayer1 + " Player 2 Score: " + playSessionScorePlayer2);
        ResetPlayerPos();

        if (!IsServer) return;

        var SQLite = GetComponent<DatabaseSQLITE>();
        SQLite.CreateMatchHistory(playSessionScorePlayer1, playSessionScorePlayer2);

        ReturnToMenuClientRpc();
        Lobby.Instance.ShutDownHost();
    }

    [ClientRpc]
    void ReturnToMenuClientRpc()
    {
        if (IsServer) return;
        Lobby.Instance.ShutDownClient();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void TransferScore(int player1, int player2)
    {
            playSessionScorePlayer1 = player1;
            playSessionScorePlayer2 = player2;
    }

    void PauseGame()
    {
        Time.timeScale = 0;
    }

    void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void ToggleLogOn()
    {
        ClearLog();
        logScreen.SetActive(true);
        Score.SetActive(false);
        UpdatePlayLogPanel();
    }

    public void ToggleLogOff()
    {
        ClearLog();
        logScreen.SetActive(false);
        Score.SetActive(true);
    }

    public void ClearLog()
    {
        foreach (Transform child in playLogContainer)
        {
            Destroy(child.gameObject);
        }
        Debug.Log("Log Screen Cleared");
    }

    public void UpdatePlayLogPanel()
    {
        var PlayLog = GetComponent<PlayLog>();
        var _playLog = PlayLog.ReadPLayLog();
        foreach (var item in _playLog)
        {
            var line = Instantiate(playLogTextPrefab, playLogContainer);
            line.GetComponent<TextMeshProUGUI>().text = item;
        }
    }


    /////////////////////////////////////
    /// NETWORK PART
    /////////////////////////////////////

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        Debug.Log("Client connected: " + clientId);

        if (!IsServer) return;
        // Optional: only spawn when a non-host client joins
        if (clientId != NetworkManager.Singleton.LocalClientId)
        {
            SpawnBall();
        }
    }

    void SpawnBall()
    {
        GameObject ballObject = Instantiate(ball, Vector3.zero, Quaternion.identity);
        ballObject.GetComponent<NetworkObject>().Spawn(true);
    }

    public override void OnNetworkDespawn()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        }
    }

}
