using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ScoreManager : NetworkBehaviour
{

    public TextMeshProUGUI Score1;
    public TextMeshProUGUI Score2;

    public NetworkVariable<int> player1Score = new NetworkVariable<int>();
    public NetworkVariable<int> player2Score = new NetworkVariable<int>();

    int maxPoints = 10;


    private void OnEnable()
    {
        Ball.OnGoalScored += HandleGoal;
    }
    private void OnDisable()
    {
        Ball.OnGoalScored -= HandleGoal;
    }

    // OnNetworkSpawn subscribe OnScoreChange and updateUI
    public override void OnNetworkSpawn()
    {
        Score1 = GameObject.Find("Score1").GetComponent<TextMeshProUGUI>();
        Score2 = GameObject.Find("Score2").GetComponent<TextMeshProUGUI>();

        player1Score.OnValueChanged += OnScoreChanged;
        player2Score.OnValueChanged += OnScoreChanged;

        UpdateUI(); // important: set initial values

        if (!IsServer) return;

        Reset();
    }

    // Update UI
    void OnScoreChanged(int oldValue, int newValue)
    {
        Debug.Log("Something A");
        UpdateUI();
    }

    // Update UI
    private void UpdateUI()
    {
        Debug.Log("Something B");
        Score1.text = player1Score.Value.ToString();
        Score2.text = player2Score.Value.ToString();
    }

    // Handle goal
    void HandleGoal(int player)
    {
        if (!NetworkManager.Singleton.IsServer) return;

        if (player == 1) OneScore();
        if (player == 2) TwoScore();
    }


    public void OneScore()
    {
        player1Score.Value++;
        if (player1Score.Value >= maxPoints)
        {
            GameManager.instance.TransferScore(player1Score.Value, player2Score.Value);
            GameManager.instance.OnPlaySessionEnded.Invoke();
            Reset();
            return;
        }
    }

    public void TwoScore()
    {
        player2Score.Value++;
        if (player2Score.Value >= maxPoints)
        {
            GameManager.instance.TransferScore(player1Score.Value, player2Score.Value);
            GameManager.instance.OnPlaySessionEnded.Invoke();
            Reset();
            return;
        }
    }

    public void Reset()
    {
        if (!NetworkManager.Singleton.IsServer) return;

        player1Score.Value = 0;
        player2Score.Value = 0;
        UpdateUI();
    }

    public override void OnNetworkDespawn()
    {
        player1Score.OnValueChanged -= OnScoreChanged;
        player2Score.OnValueChanged -= OnScoreChanged;

        Ball.OnGoalScored -= HandleGoal;
    }

}
