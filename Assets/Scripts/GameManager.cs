using TMPro;
using UnityEngine;
using UnityEngine.Events;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject PlayerPrefab;
    public ScoreManager scoreManager;

    public UnityEvent OnPlaySessionEnded = new UnityEvent();
    public UnityEvent OnPlaySessionStarted = new UnityEvent();

    public GameObject playLogTextPrefab;
    public Transform playLogContainer;

    public GameObject logScreen;

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

    GameObject player1;
    GameObject player2;
    GameObject ball;
    GameObject scoreBoard;

    public int playSessionScorePlayer1;
    public int playSessionScorePlayer2;
    public int ballHits;

    public GameObject LOBBY_UI;

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
    void Start()
    {
        SetupScene();
    }

   

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetupScene()
    {
        player1 = Instantiate(PlayerPrefab, player1SpawnPos.position, Quaternion.identity);
        player1.GetComponent<PlayerController>().isPlayer1 = true;
        player1.GetComponent<PlayerController>().isOwner = true;

        player2 = Instantiate(PlayerPrefab, player2SpawnPos.position, Quaternion.identity);
        player2.GetComponent<PlayerController>().isOwner = true;

        ball = GameObject.FindWithTag("Ball");
        ball.GetComponent<Ball>().players.Add(player1);
        ball.GetComponent<Ball>().players.Add(player2);
        var logscript = GetComponent<PlayLog>();
        logscript.UpdatePlaySessionCount();
    }

    public void ResetScene()
    {
        Destroy(player1);
        Destroy(player2);
        scoreBoard = GameObject.FindWithTag("ScoreBoard");
        scoreBoard.GetComponent<ScoreManager>().Reset();
        SetupScene();
        ballHits = 0;
        ResumeGame();
    }

    public void ResetPlayerPos()
    {
        player1.transform.position = player1SpawnPos.position;
        player2.transform.position = player2SpawnPos.position;
    }

    public void EndGameOfPong()
    {
        Debug.Log("Game Ended! Player 1 Score: " + playSessionScorePlayer1 + " Player 2 Score: " + playSessionScorePlayer2);
        Destroy(player1);
        Destroy(player2);
        PauseGame();
        var SQLite = GetComponent<DatabaseSQLITE>();
        SQLite.CreateMatchHistory(playSessionScorePlayer1, playSessionScorePlayer2);
        LOBBY_UI.SetActive(true);
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

    public void StartGame()
    {
        LOBBY_UI.SetActive(false);
        OnPlaySessionStarted.Invoke();
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
        UpdatePlayLogPanel();
    }
    
    public void ToggleLogOff()
    {
        ClearLog();
        logScreen.SetActive(false);
    }

    public void ClearLog()
    {
        foreach (Transform child in playLogContainer)
        {
            Destroy(child.gameObject);
        }
        Debug.Log("Log Screen Cleared");
    }
}
