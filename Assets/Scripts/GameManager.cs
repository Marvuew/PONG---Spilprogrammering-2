using UnityEngine;
using UnityEngine.Events;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject PlayerPrefab;
    public Ball ball;

    public UnityEvent OnPlaySessionEnded = new UnityEvent();
    public UnityEvent OnPlaySessionStarted = new UnityEvent();

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
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void OnEnable()
    {
        OnPlaySessionEnded.AddListener();
        OnPlaySessionStarted.AddListener(SetUpScene);

    }

    public void OnDisable()
    {
        OnPlaySessionEnded.RemoveAllListeners();
        OnPlaySessionStarted.RemoveAllListeners();
    }
    void Start()
    {
        SpawnPlayers();
    }

   

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnPlayers()
    {
        player1 = Instantiate(PlayerPrefab, player1SpawnPos.position, Quaternion.identity);
        player1.GetComponent<PlayerController>().isPlayer1 = true;
        player1.GetComponent<PlayerController>().isOwner = true;
        ball.players.Add(player1);
        player2 = Instantiate(PlayerPrefab, player2SpawnPos.position, Quaternion.identity);
        player2.GetComponent<PlayerController>().isOwner = true;
        ball.players.Add(player2);
    }

    public void SetUpScene()
    {
        player1.transform.position = player1SpawnPos.position;
        player2.transform.position = player2SpawnPos.position;
    }
}
